
using System;
using System.Media;
using System.Collections.ObjectModel;

using TimeTracker.Models;
using TimeTracker.Helpers;
using TimeTracker.Service;
using TimeTracker.EventBus;
using TimeTracker.Notifications;

namespace TimeTracker.ViewModels
{
    public class NotificationViewModel : DomainModelBase
    {
        public NotificationViewModel(ApplicationEnvironment appApplicationEnvironment)
        {
            _appApplicationEnvironment = appApplicationEnvironment;

            _popupNotifier = new PopupNotifier();

            Notifications = new ObservableCollection<NotificationDataModel>();
            ClearAllNotificationsCommand = new RelayCommand(ClearAllNotifications);

            CreateEventHubSubscriptions();
        }

        #region Props and Fields

        public RelayCommand ClearAllNotificationsCommand { get; set; }

        private ApplicationEnvironment _appApplicationEnvironment;
        public ObservableCollection<NotificationDataModel> Notifications { get; set; }

        private bool _needPopupNotify;
        private bool _needSoundNotify;

        private PopupNotifier _popupNotifier;

        private bool _isViewVisible;
        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set { SetProperty(ref _isViewVisible, value); }
        }

        #endregion

        private void CreateEventHubSubscriptions()
        {
            _appApplicationEnvironment.EventHub.Subscribe<ShowNotificationWindowMessage>(OnShowNoficationWindowHandler);
            _appApplicationEnvironment.EventHub.Subscribe<NotificationMessage>(PushNotification);
            _appApplicationEnvironment.EventHub.Subscribe<ChangeSettingsMessage>(ChangeSettingsHandler);
        }

        public void OnCloseNotificationWindowHandler()
        {
            IsViewVisible = false;
        }

        private void ChangeSettingsHandler(ChangeSettingsMessage m)
        {
            _needPopupNotify = m.NeedPopupNotify;
            _needSoundNotify = m.NeedSoundNotify;
        }

        private void PushNotification(NotificationMessage m)
        {
            Notifications.Add(new NotificationDataModel() { NotificationMessage = m.Text, TimeStamp = m.TimeStamp, IsReaded = false });
            _appApplicationEnvironment.EventHub.Publish<UpdateNotifyBadgeMessage>(new UpdateNotifyBadgeMessage() { Value = Notifications.Count });

            if (_needPopupNotify)
            {
                _popupNotifier.ShowPopupMessage("Time Tracker", m.Text);
            }

            if (_needSoundNotify)
            {
                SystemSounds.Asterisk.Play(); //temp
            }
        }

        private void OnShowNoficationWindowHandler(ShowNotificationWindowMessage m)
        {
            IsViewVisible = true;
        }

        public void ClearAllNotifications()
        {
            Notifications.Clear();
            _appApplicationEnvironment.EventHub.Publish<UpdateNotifyBadgeMessage>(new UpdateNotifyBadgeMessage() { Value = Notifications.Count });
        }
    }
}
