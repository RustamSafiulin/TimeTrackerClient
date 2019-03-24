
using System;
using Microsoft.Win32;

namespace TimeTracker.Helpers
{
    public static class RegistryHelpers
    {
        public static T GetValue<T>(string registryKeyPath, string value, T defaultValue = default(T))
        {
            T retVal = default(T);

            retVal = (T)Registry.GetValue(registryKeyPath, value, defaultValue);

            return retVal;
        }
    }
}
