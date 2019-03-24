using System.Collections.Generic;
using System.Linq;
using ToastNotifications.Core;

namespace ToastNotifications.Lifetime.Clear
{
    public class ClearByMessage : IClearStrategy
    {
        private readonly string _message;

        public ClearByMessage(string message)
        {
            _message = message;
        }

        public IEnumerable<INotification> GetNotificationsToRemove(NotificationsList notifications)
        {
            var notificationsToRemove = notifications
                .Select(x => x.Value.Notification)
                .Where(x => x.DisplayPart.GetMessage() == _message)
                .ToList();

            return notificationsToRemove;
        }
    }
}