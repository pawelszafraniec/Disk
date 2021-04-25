using Microsoft.Extensions.Configuration;

namespace Disk.Common
{
    public static class IConfigurationExtensions
    {
        public static string GetRequiredValue(this IConfiguration configuration, string key)
        {
            return configuration[key] ?? throw new ConfigurationMissingException(GetRealKey(configuration, key));
        }

        private static string GetRealKey(IConfiguration configuration, string key)
        {
            return configuration switch
            {
                IConfigurationSection configurationSection => $"{configurationSection.Path}:{key}",
                _ => key,
            };
        }
    }
}
