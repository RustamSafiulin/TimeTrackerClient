using System.Collections.Generic;
using System.Linq;
using ToastNotifications.Core;

namespace ToastNotifications.Lifetime.Clear
{
    public class ClearByTag : IClearStrategy
    {
        private readonly object _tag;

        public ClearByTag(object tag)
        {
            _tag = tag;
        }

        public IEnumerable<INotification> GetNotificationsToRemove(NotificationsList notifications)
        {
            var notificationsToRemove = notifications
                .Where(x => x.Value.Notification.DisplayPart.GetOptions().Tag == _tag)
                .Select(x => x.Value.Notification)
                .ToList();

            return notificationsToRemove;
        }
    }
}