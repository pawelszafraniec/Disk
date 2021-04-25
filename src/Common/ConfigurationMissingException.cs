using System;

namespace Disk.Common
{
    class ConfigurationMissingException : Exception
    {
        public string Key { get; }

        public ConfigurationMissingException(string key)
        {
            this.Key = key;
        }
    }
}
