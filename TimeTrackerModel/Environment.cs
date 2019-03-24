
using System;

using TimeTracker.EventBus;
using TimeTracker.Service;
using TimeTracker.Helpers;

namespace TimeTracker
{
    public class ApplicationEnvironment
    {
        public ApplicationEnvironment()
        { }

        #region Props and Fields

        public Configuration AppConfiguration { get; set; }
        public TinyMessengerHub EventHub { get; set; }
        public ServiceConnector ServiceConnector { get; set; }

        #endregion

        public void SetupApplicationEnvironment()
        {
            Logger.InitLogger();

            InitConfiguration();
            InitEventHub();
            InitServiceConnector();
        }

        private void InitConfiguration()
        {
            Logger.Log.Info("Read stored configuration parameters");

            AppConfiguration = new Configuration();
            AppConfiguration.ParseConfiguration();
        }

        private void InitEventHub()
        {
            Logger.Log.Info("Initialize Event Hub");

            EventHub = new TinyMessengerHub();
        }

        private void InitServiceConnector()
        {
            Logger.Log.Info("Initialize Web Service connector");

            ServiceConnector = new ServiceConnector(new Options { BaseAddress = AppConfiguration.StoredParameters.ServiceBaseAddress, ExchangeFormat = RestSharp.DataFormat.Json });

            ServiceConnector.RegisterRoute(OperationType.Signup, new Route { Path = "api/v1/signup" });
            ServiceConnector.RegisterRoute(OperationType.Signin, new Route { Path = "api/v1/signin" });
            ServiceConnector.RegisterRoute(OperationType.Signout, new Route { Path = "api/v1/logout" });
            ServiceConnector.RegisterRoute(OperationType.ResetPassword, new Route { Path = "api/v1/reset_password" });
            ServiceConnector.RegisterRoute(OperationType.UpdateProfileInfo, new Route { Path = "" });
            ServiceConnector.RegisterRoute(OperationType.GetProfileInfo, new Route { Path = "api/v1/profiles/{id}" });
            ServiceConnector.RegisterRoute(OperationType.CreateActivity, new Route { Path = "api/v1/activities" });
            ServiceConnector.RegisterRoute(OperationType.DeleteActivity, new Route { Path = "api/v1/activities/{id}" });
            ServiceConnector.RegisterRoute(OperationType.UpdateActvity, new Route { Path = "api/v1/activities/{id}" });
            ServiceConnector.RegisterRoute(OperationType.GetActivity, new Route { Path = "api/v1/activities/{id}" });
            ServiceConnector.RegisterRoute(OperationType.GetActivities, new Route { Path = "api/v1/activities"});
            ServiceConnector.RegisterRoute(OperationType.UpdateSettings, new Route { Path = "api/v1/profiles/{id}/settings" });
            ServiceConnector.RegisterRoute(OperationType.GetSettings, new Route { Path = "api/v1/profiles/{id}/settings" });
            ServiceConnector.RegisterRoute(OperationType.UpdateProfileAvatar, new Route { Path = "api/v1/profiles/{id}/avatar" });
            ServiceConnector.RegisterRoute(OperationType.GetProfileAvatar, new Route { Path = "api/v1/profiles/{id}/avatar" });
        }
    }
}
