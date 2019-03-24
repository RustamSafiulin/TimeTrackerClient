using System.Collections.Generic;
using System.Linq;
using ToastNotifications.Core;

namespace ToastNotifications.Lifetime.Clear
{
    public class ClearAll: IClearStrategy
    {
        public IEnumerable<INotification> GetNotificationsToRemove(NotificationsList notifications)
        {
            return notifications.Select(x => x.Value.Notification);
        }
    }
}