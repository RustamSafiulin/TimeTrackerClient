using System;
using ToastNotifications.Core;

namespace ToastNotifications.Lifetime
{
    public class CloseNotificationEventArgs : EventArgs
    {
        public INotification Notification { get; }

        public CloseNotificationEventArgs(INotification notification)
        {
            Notification = notification;
        }
    }
}