using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disk.Api.Configuration
{
    public static class LoggingConfiguration
    {
        public static void ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(config =>
            {
                config.AddConfiguration(configuration);

                config.AddConsole(consoleConfig =>
                {
                    consoleConfig.TimestampFormat = "[HH:mm:ss] ";
                });
            });
        }
    }
}
