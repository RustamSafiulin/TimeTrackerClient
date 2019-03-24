using System;
using System.Linq;

using System.Net;
using System.Diagnostics;
using System.Windows;

using TimeTracker.ViewModels;
using TimeTracker.Views;

namespace TimeTracker
{
    public partial class App : Application
    {
        private ApplicationController _appController;

        private void ApplicationStartupHandler(object sender, StartupEventArgs e)
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

                _appController = new ApplicationController();
                _appController.Startup();
            }
            catch (AggregateException ae)
            {
                var stackErrors = String.Empty;
                foreach (var exception in ae.Flatten().InnerExceptions)
                {
                    stackErrors += String.Format("{0}{1}", exception.Message, Environment.NewLine);
                }

                MessageBox.Show("Strings.exceptionReason + stackErrors", "Strings.exceptionErrorAppInitialization");
                Application.Current.Shutdown();
            }
            catch (AppAlreadyStartedException exception)
            {
                MessageBox.Show(exception.Message, ""/*Strings.exceptionErrorAppInitialization*/);

                var process = Process.GetProcesses().Where(p => (p.ProcessName.Equals("Manager") && p.Id != Process.GetCurrentProcess().Id)).FirstOrDefault();

                if (process != null)
                {
                    WindowExtensions.ShowWindow(process.MainWindowHandle, WindowExtensions.SW_RESTORE);
                    WindowExtensions.SetForegroundWindow(process.MainWindowHandle);
                }

                Application.Current.Shutdown();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, /*Strings.exceptionErrorAppInitialization*/ "");
                Application.Current.Shutdown();
            }
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            var exception = args.ExceptionObject as Exception;

            if (exception != null)
            {
                Debug.WriteLine(String.Format("{0}, {1}", exception.Message, exception.StackTrace));
                MessageBox.Show(exception.Message);
                Application.Current.Shutdown();
            }
        }

        private void ApplicationExitHandler(object sender, ExitEventArgs e)
        {
            //_appController.DeInitialize();
            Debug.WriteLine("Uss GUI exit with code: {0}", e.ApplicationExitCode);
        }

        private void ApplicationNavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {
            if (e.Exception is WebException)
            {
                //MessageBox.Show(Strings.exceptionSiteIsNotAvailable + e.Uri.ToString());
                e.Handled = true;
            }
        }
    }
}
