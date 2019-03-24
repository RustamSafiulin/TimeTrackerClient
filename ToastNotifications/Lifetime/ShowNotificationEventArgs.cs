using System;
using ToastNotifications.Core;

namespace ToastNotifications.Lifetime
{
    public class ShowNotificationEventArgs : EventArgs
    {
        public INotification Notification { get; }

        public ShowNotificationEventArgs(INotification notification)
        {
            Notification = notification;
        }
    }
}