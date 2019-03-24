using System.Windows;
using ToastNotifications.Core;
using ToastNotifications.Messages.Core;

namespace ToastNotifications.Messages.Error
{
    public class ErrorMessage : MessageBase<ErrorDisplayPart>
    {
        public ErrorMessage(string message) : this(message, new MessageOptions())
        {
        }

        public ErrorMessage(string message, MessageOptions options) : base(message, options)
        {
        }

        protected override ErrorDisplayPart CreateDisplayPart()
        {
            return new ErrorDisplayPart(this);
        }

        protected override void UpdateDisplayOptions(ErrorDisplayPart displayPart, MessageOptions options)
        {
            if (options.FontSize != null)
                displayPart.Text.FontSize = options.FontSize.Value;

            displayPart.CloseButton.Visibility = options.ShowCloseButton ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}