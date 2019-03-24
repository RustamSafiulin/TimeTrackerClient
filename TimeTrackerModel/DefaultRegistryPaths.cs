
using System;

using TimeTracker.Helpers;

namespace TimeTracker
{
    public static class DefaultRegistryValues
    {
        private static readonly string AppRootKeyPath = @"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Tracker Apps\Time Tracker";

        public static string GetLogsDir()
        {
            var result = RegistryHelpers.GetValue<string>(AppRootKeyPath, "LogsDir");
            return result;
        }

        public static string GetConfigsDir()
        {
            var result = RegistryHelpers.GetValue<string>(AppRootKeyPath, "ConfigsDir");
            return result;
        }
    }
}
