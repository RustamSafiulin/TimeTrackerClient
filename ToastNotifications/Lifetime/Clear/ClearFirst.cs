using System.Collections.Generic;
using System.Linq;
using ToastNotifications.Core;

namespace ToastNotifications.Lifetime.Clear
{
    public class ClearFirst : IClearStrategy
    {
        public IEnumerable<INotification> GetNotificationsToRemove(NotificationsList notifications)
        {
            if (notifications.IsEmpty)
            {
                return Enumerable.Empty<INotification>();
            }

            var lastMessage = notifications.FirstOrDefault().Value.Notification;

            return new[] { lastMessage };
        }
    }
}