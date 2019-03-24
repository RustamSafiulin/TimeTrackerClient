using System;

namespace ToastNotifications.Core
{
    public abstract class NotificationBase : INotification
    {
        private Action<INotification> _closeAction;

        public bool CanClose { get; set; } = true;

        public abstract NotificationDisplayPart DisplayPart { get; }

        public int Id { get; set; }

        public virtual void Bind(Action<INotification> closeAction)
        {
            _closeAction = closeAction;
        }

        public virtual void Close()
        {
            var opts = DisplayPart.GetOptions() as MessageOptions;
            if (opts != null && opts.CloseClickAction != null)
            {
                opts.CloseClickAction(this);
            }
            _closeAction?.Invoke(this);
        }

        public string GetMessage()
        {
            return DisplayPart.GetMessage();
        }
    }
}