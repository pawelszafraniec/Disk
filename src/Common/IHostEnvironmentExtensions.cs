using Microsoft.Extensions.Hosting;

using System;

namespace Disk.Common
{
    public static class IHostEnvironmentExtensions
    {
        public static bool IsLocal(this IHostEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            return environment.IsEnvironment("Local");
        }
    }
}
