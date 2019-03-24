
using System;
using System.Collections.ObjectModel;
using System.Threading;

using TimeTracker;
using TimeTracker.EventBus;
using TimeTracker.Helpers;
using TimeTracker.Service;

namespace TimeTracker.ViewModels
{
    public class ContextMenuCommand
    {
        public String Title { get; set; }
        public RelayCommand Command { get; set; }
    }

    public class ApplicationViewModel : DomainModelBase
    {
        public ApplicationViewModel(ApplicationEnvironment appApplicationEnvironment)
        {
            _appApplicationEnvironment = appApplicationEnvironment;
            CreateEventHubSubscriptions();

            ShowNotificationWindowCommand = new RelayCommand(ShowNotificationWindowCommandHandler);
            ShowProfileSettingsCommand = new RelayCommand(ShowProfileSettingsWindowCommandHandler);
            LogoutCommand = new RelayCommand(LogoutCommandHandler);

            ContextMenuCommands = new ObservableCollection<ContextMenuCommand>()
            {
                new ContextMenuCommand() {Title = "Профиль", Command = ShowProfileSettingsCommand },
                new ContextMenuCommand() {Title = "Выйти", Command = LogoutCommand }
            };
        }

        #region Props and Fields

        private ApplicationEnvironment _appApplicationEnvironment;

        public RelayCommand ShowNotificationWindowCommand { get; set; }
        public RelayCommand ShowProfileSettingsCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }

        public ObservableCollection<ContextMenuCommand> ContextMenuCommands { get; private set; }

        private string _notificationsCount = "0";
        public string NotificationsCount
        {
            get { return _notificationsCount; }
            set { SetProperty(ref _notificationsCount, value); }
        }

        private bool _isViewVisible;
        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set { SetProperty(ref _isViewVisible, value); }
        }
        
        #endregion

        private void CreateEventHubSubscriptions()
        {
            _appApplicationEnvironment.EventHub.Subscribe<LoginMessage>(OnLoginHandler);
            _appApplicationEnvironment.EventHub.Subscribe<SignupMessage>(OnSignupHandler);
            _appApplicationEnvironment.EventHub.Subscribe<UpdateNotifyBadgeMessage>(OnNotifyBadgeUpdateHandler);
        }

        public void CheckAppAlreadyStartedInCurrentSession()
        {
            Boolean result;
            var sessionOnlyCheckMtxName = String.Join("Local\\us_manager_one_instance_mtx_", System.Security.Principal.WindowsIdentity.GetCurrent().Name.Replace("\\", "_"));

            var singleApplicationMutex = new Mutex(true, sessionOnlyCheckMtxName, out result);

            if (!result)
                throw new AppAlreadyStartedException("Already started");
        }

        public void OnNotifyBadgeUpdateHandler(UpdateNotifyBadgeMessage m)
        {
            NotificationsCount = Convert.ToString(m.Value);
        }

        public void OnLoginHandler(LoginMessage m)
        {
            SessionStorage.Instance.ProfileId = m.SessionInfo.ProfileId;
            SessionStorage.Instance.SessionId = m.SessionInfo.SessionId;

            IsViewVisible = true;

            _appApplicationEnvironment.EventHub.Publish<NeedServerSyncMessage>(new NeedServerSyncMessage());
        }

        public void OnAppNeedExitHandler()
        {
            _appApplicationEnvironment.EventHub.Publish(new AppExitMessage());
        }

        public void OnSignupHandler(SignupMessage m)
        { }

        private void ShowNotificationWindowCommandHandler()
        {
            _appApplicationEnvironment.EventHub.Publish(new ShowNotificationWindowMessage());
        }

        private void ShowProfileSettingsWindowCommandHandler()
        {
            _appApplicationEnvironment.EventHub.Publish(new ShowProfileSettingsWindowMessage());
        }

        private void LogoutCommandHandler()
        {
            IsViewVisible = false;
            _appApplicationEnvironment.EventHub.Publish(new LogoutMessage());
        }
    }
}
