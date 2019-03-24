
using ToastNotifications.Core;

namespace TimeTracker.Notifications
{
    public partial class CustomDisplayPart : NotificationDisplayPart
    {
        private CustomNotification _customNotification;

        public CustomDisplayPart(CustomNotification customNotification)
        {
            _customNotification = customNotification;
            DataContext = customNotification;

            InitializeComponent();
        }
    }
}
