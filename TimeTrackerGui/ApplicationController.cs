
using System;
using System.Diagnostics;
using System.Windows;

using TimeTracker.Service;
using TimeTracker.ViewModels;
using TimeTracker.Views;
using TimeTracker.Helpers;
using TimeTracker.EventBus;

namespace TimeTracker
{
    public sealed class ApplicationController
    {
        #region Props and Fields

        private EntryViewModel _entryModel;
        private EntryWindow _entryWindow;

        private TimersViewModel _timersViewModel;
        private TimersViewPage _timersViewPage;

        private ActivityDetailsWindow _activityDetailsWindow;

        private ReportsViewModel _reportsViewModel;
        private ReportsViewPage _reportsViewPage;

        private SettingsViewModel _settingsViewModel;
        private SettingsViewPage _settingsViewPage;

        private NotificationWindow _notificationWindow;
        private NotificationViewModel _notificationViewModel;

        private ProfileSettingsWindow _profileSettingsWindow;
        private ProfileSettingsViewModel _profileSettingsViewModel;

        private MainWindow _mainView;
        private ApplicationViewModel _appViewModel;

        private IDialogService _dialogService;

        private ApplicationEnvironment _appApplicationEnvironment;

        #endregion

        public ApplicationController()
        { }

        public void Startup()
        {
            _appApplicationEnvironment = new ApplicationEnvironment();
            _appApplicationEnvironment.SetupApplicationEnvironment();

            _appApplicationEnvironment.EventHub.Subscribe<AppExitMessage>(OnAppShutdownHandler);

            Logger.Log.Info("Init data models");
            _entryModel = new EntryViewModel(_appApplicationEnvironment);
            _appViewModel = new ApplicationViewModel(_appApplicationEnvironment);
            _timersViewModel = new TimersViewModel(_appApplicationEnvironment);
            _reportsViewModel = new ReportsViewModel(_appApplicationEnvironment);
            _settingsViewModel = new SettingsViewModel(_appApplicationEnvironment);
            _notificationViewModel = new NotificationViewModel(_appApplicationEnvironment);
            _profileSettingsViewModel = new ProfileSettingsViewModel(_appApplicationEnvironment);

            Logger.Log.Info("Init dialog service");
            _dialogService = new DialogService();
            _dialogService.RegisterDialog<ConfirmationDialogModel, ConfirmationDialog>();

            Logger.Log.Info("Init views");
            _mainView = new MainWindow(_dialogService, _appViewModel);

            _entryWindow = new EntryWindow(_entryModel);

            _activityDetailsWindow = new ActivityDetailsWindow(_mainView, _timersViewModel);

            _notificationWindow = new NotificationWindow(_mainView, _notificationViewModel);
            _profileSettingsWindow = new ProfileSettingsWindow(_mainView, _profileSettingsViewModel);

            _timersViewPage = new TimersViewPage(_timersViewModel);
            _mainView.AddPage(_timersViewPage);

            _reportsViewPage = new ReportsViewPage(_reportsViewModel);
            _mainView.AddPage(_reportsViewPage);

            _settingsViewPage = new SettingsViewPage(_settingsViewModel);
            _mainView.AddPage(_settingsViewPage);

            _mainView.Run();
        }

        private void OnAppShutdownHandler(AppExitMessage m)
        {
            _appApplicationEnvironment.AppConfiguration.SaveConfiguration();
            Application.Current.Shutdown();
        }
    }
}
