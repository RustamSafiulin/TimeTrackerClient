
using System;
using System.IO;
using System.Diagnostics;

using TimeTracker.Helpers;
using TimeTracker.Models;

namespace TimeTracker
{
    public class Configuration
    {
        public Configuration()
        { }

        private readonly string _configFileName = "app_params.conf";

        private readonly AppStoredParameters DefaultStoredParams = new AppStoredParameters()
        {
            RememberMe = false,
            Email = "",
            Password = "",
            ServiceBaseAddress = "http://127.0.0.1:3000"
        };

        public AppStoredParameters StoredParameters { get; set; }

        public void ParseConfiguration()
        {
            AppStoredParameters appStoredParams = default(AppStoredParameters);
            bool isError = false;

            try
            {
                var configFilePath = string.Empty;
                var configsDir = DefaultRegistryValues.GetConfigsDir();
                if (configsDir != null)
                {
                    configFilePath = Path.Combine(configsDir, _configFileName);
                }
                else
                {
                    configFilePath = _configFileName;
                }

                if (!File.Exists(configFilePath))
                {
                    throw new FileNotFoundException($"Config file {configFilePath} doesn't exist");
                }

                appStoredParams = SerializationHelpers.ReadFromBinaryFile<AppStoredParameters>(configFilePath);
            }
            catch (Exception e)
            {
                Logger.Log.Debug($"Error during reading configuration from file. Error text: {e.Message}");
                isError = true;
            }

            if (isError)
            {
                StoredParameters = DefaultStoredParams;
                return;
            }

            if (appStoredParams != null)
            {
                StoredParameters = appStoredParams;
                return;
            }

            StoredParameters = DefaultStoredParams;
        }

        public void SaveConfiguration()
        {
            try
            {
                var configFilePath = string.Empty;
                var configsDir = DefaultRegistryValues.GetConfigsDir();
                if (configsDir != null)
                {
                    configFilePath = Path.Combine(configsDir, _configFileName);
                }
                else
                {
                    configFilePath = _configFileName;
                }

                StoredParameters.WriteToBinaryFile(configFilePath);
            }
            catch (Exception e)
            {
                Logger.Log.Debug($"Error during save configuration to file. Error text: {e.Message}");
            }
        }
    }
}
