
using System;

using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace TimeTracker.Notifications
{
    public class PopupNotifier : IDisposable
    {
        public PopupNotifier()
        {
            _notifier = new Notifier(cfg =>
            {
                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(TimeSpan.FromSeconds(15), MaximumNotificationCount.FromCount(15));
                cfg.PositionProvider = new PrimaryScreenPositionProvider(Corner.BottomRight, 10, 10);
            });
        }

        private readonly Notifier _notifier;

        private bool disposed = false;

        public void ShowPopupMessage(string title, string message)
        {
            _notifier.Notify<CustomNotification>(() => new CustomNotification(title, message));
        }

        public void Dispose()
        {
            Cleanup(true);
            GC.SuppressFinalize(this);
        }

        private void Cleanup(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _notifier.Dispose();
                }
            }
            disposed = true;
        }

        ~PopupNotifier()
        {
            Cleanup(false);
        }
    }
}
