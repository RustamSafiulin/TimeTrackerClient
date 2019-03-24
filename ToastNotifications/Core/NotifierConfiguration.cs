using System.Windows.Threading;
using ToastNotifications.Events;
using ToastNotifications.Lifetime;

namespace ToastNotifications.Core
{
    public class NotifierConfiguration
    {
        public IPositionProvider PositionProvider { get; set; }
        public INotificationsLifetimeSupervisor LifetimeSupervisor { get; set; }
        public Dispatcher Dispatcher { get; set; }
        public DisplayOptions DisplayOptions { get; }
        public IKeyboardEventHandler KeyboardEventHandler { get; set; }

        public NotifierConfiguration()
        {
            DisplayOptions = new DisplayOptions
            {
                Width = 250,
                TopMost = true
            };
        }
    }
}