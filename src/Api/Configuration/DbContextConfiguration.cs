using Disk.Api.Data;
using Disk.Search;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Disk.Api.Configuration
{
    public static class DbContextConfiguration
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection databaseConfiguration = configuration.GetSection("App:Database");

            string connectionString = databaseConfiguration["ConnectionString"];
            string provider = databaseConfiguration["Provider"];

            switch (provider.ToLower())
            {
                case "postgresql":
                    services.AddDbContext<ApplicationDbContext>(configuration =>
                    {
                        configuration.UseNpgsql(connectionString);
                    });
                    break;

                case "sqlite":
                    services.AddDbContext<ApplicationDbContext>(configuration =>
                    {
                        configuration.UseSqlite(connectionString);
                    });
                    break;
                default:
                    break;
            }


            services.AddExpressionParser<ApplicationDbContext>();
        }
    }
}
