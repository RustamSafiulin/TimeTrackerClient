using System.Windows;
using System.Windows.Input;
using ToastNotifications.Core;

namespace ToastNotifications.Messages.Core
{
    public abstract class MessageBase<TDisplayPart> : NotificationBase where TDisplayPart : NotificationDisplayPart
    {
        protected NotificationDisplayPart _displayPart;
        internal readonly MessageOptions Options;

        public string Message { get; }

        public MessageBase(string message, MessageOptions options)
        {
            Message = message;
            if (options == null)
                Options = new MessageOptions();
            else
                Options = options;
        }

        public override NotificationDisplayPart DisplayPart => _displayPart ?? (_displayPart = Configure());

        private TDisplayPart Configure()
        {
            TDisplayPart displayPart = CreateDisplayPart();

            displayPart.Unloaded += OnUnloaded;
            displayPart.MouseLeftButtonDown += OnLeftMouseDown;

            UpdateDisplayOptions(displayPart, Options);
            return displayPart;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _displayPart.MouseLeftButtonDown -= OnLeftMouseDown;
            _displayPart.Unloaded -= OnUnloaded;
        }

        private void OnLeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            Options.NotificationClickAction?.Invoke(this);
        }

        protected abstract void UpdateDisplayOptions(TDisplayPart displayPart, MessageOptions options);

        protected abstract TDisplayPart CreateDisplayPart();
    }
}