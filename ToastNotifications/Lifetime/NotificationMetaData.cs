using System;
using ToastNotifications.Core;

namespace ToastNotifications.Lifetime
{
    public struct NotificationMetaData
    {
        public INotification Notification { get; }
        public int Id { get; }
        public TimeSpan CreateTime { get; }

        public NotificationMetaData(INotification notification, int id, TimeSpan createTime)
        {
            Notification = notification;
            Notification.Id = id;
            Id = id;
            CreateTime = createTime;
        }
    }
}