using FdkMinimal;
using FdkMinimal.Facilities;
using FdkRHost.Logging;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using SoftFX.Extended.Financial;
using System;
using System.IO;
using System.Reflection;

namespace RHost
{
    public static class FdkStatic
    {
        private static FinancialCalculator _calculator;
        public static FinancialCalculator Calculator
        {
            get
            {
                if (_calculator == null)
                {
                    _calculator = new FinancialCalculator();
                    _calculator.Init(FdkHelper.Feed);
                }
                return _calculator;
            }
            private set { _calculator = value; }
        }
        static FdkStatic()
        {
            Calculator = new FinancialCalculator();
            SetupLog4Net();
        }

        static void SetupLog4Net()
        {
            // Configure log4net.
            var locateFileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var parentPath = new FileInfo(Path.Combine(locateFileInfo.DirectoryName, "app.config"));
            XmlConfigurator.Configure(parentPath);


            string logFile = @"c:\rFdkLogs\";

            var repository = (log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository(Assembly.GetCallingAssembly());
            repository.Name = "rFdk";
            // AsyncForwardingAppender
            var asyncForwardingAppender = new AsyncForwardingAppender();
            asyncForwardingAppender.Name = "AsyncForwardingAppender";
            // DebugLogFileAppender
            asyncForwardingAppender.AddAppender(CreateAppenderExactLevelInstance(logFile, " yyyy-MM-dd [HH]' Debug.log'", Level.Debug));

            // root            
            asyncForwardingAppender.ActivateOptions();
            repository.Root.AddAppender(asyncForwardingAppender);
            repository.Root.Level = Level.Info;
            repository.Configured = true;
        }

        static IAppender CreateAppenderExactLevelInstance(string logFile, string datePattern, Level level)
        {
            var exactLevelLogFileAppender = new RollingFileAppender();
            exactLevelLogFileAppender.DateTimeStrategy = new UniversalDateTime();
            exactLevelLogFileAppender.File = logFile;
            exactLevelLogFileAppender.AppendToFile = true;
            exactLevelLogFileAppender.RollingStyle = RollingFileAppender.RollingMode.Date;
            exactLevelLogFileAppender.DatePattern = datePattern;
            exactLevelLogFileAppender.StaticLogFileName = false;
            var patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%utcdate [%property{ThreadId}] %-5p %c{1} %m%n";
            patternLayout.ActivateOptions();
            exactLevelLogFileAppender.Layout = patternLayout;

            exactLevelLogFileAppender.ActivateOptions();
            return exactLevelLogFileAppender;
        }

        public static bool ConnectToFdk(string address, string login, string password, string path, string protocol = "fix")
        {
            Calculator = null;
            FdkEnvironment.AppDir = path;
            return FdkHelper.ConnectToFdk(address, login, password, protocol);
        }

        public static void Disconnect()
        {
            Calculator = null;
            FdkHelper.Disconnect();
            FdkVars.ClearAll();
        }

        public static bool IsConnected()
        {
            return FdkHelper.IsConnected;
        }

        public static void DisplayDate(DateTime time)
        {
            FdkHelper.DisplayDate(time);
        }

    }
}
