using Disk.Api.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Disk.Api.Configuration
{
    public static class HealthChecksConfiguration
    {
        public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                    .AddProcessAllocatedMemoryHealthCheck(512, "Memory usage")
                    .AddDbContextCheck<ApplicationDbContext>("Database connection");

            services.AddHealthChecksUI(setupSettings: setup =>
            {
                setup.SetHealthCheckDatabaseConnectionString("Data Source=healthcheck.disk.sqlite3");

                foreach (IConfigurationSection check in configuration.GetSection("App:HealthChecks").GetChildren())
                {
                    setup.AddHealthCheckEndpoint(check.Key, check.Value);
                }
            });
        }

        public static void UseHealthCheck(this IApplicationBuilder app)
        {
            app.UseHealthChecksUI(config =>
            {
                config.UIPath = "/health";
            });
        }
    }
}
