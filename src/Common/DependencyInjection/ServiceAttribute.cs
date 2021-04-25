using Microsoft.Extensions.DependencyInjection;

using System;

namespace Disk.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;
    }
}
