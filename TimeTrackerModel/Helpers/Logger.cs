
using System;
using System.IO;
using Microsoft.Win32;

using log4net;
using log4net.Repository.Hierarchy;
using log4net.Core;
using log4net.Appender;
using log4net.Layout;
using log4net.Config;

namespace TimeTracker.Helpers
{
    public class Logger
    {
        private static readonly ILog log = LogManager.GetLogger("LOGGER");

        private static readonly string logFileName = "TimeTracker.log";

        public static ILog Log
        {
            get { return log; }
        }

        public static void InitLogger()
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender();
            roller.AppendToFile = true;

            var logsDir = DefaultRegistryValues.GetLogsDir();
            if (logsDir != null)
            {
                roller.File = Path.Combine(logsDir, logFileName);
            }
            else
            {
                roller.File = Path.Combine("Logs", logFileName);
            }

            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = 5;
            roller.MaximumFileSize = "10MB";
            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            roller.StaticLogFileName = true;
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            hierarchy.Root.Level = Level.Info;

            BasicConfigurator.Configure(hierarchy);
        }
    }
}
