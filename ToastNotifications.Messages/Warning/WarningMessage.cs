using System.Windows;
using ToastNotifications.Core;
using ToastNotifications.Messages.Core;

namespace ToastNotifications.Messages.Warning
{
    public class WarningMessage : MessageBase<WarningDisplayPart>
    {
        public WarningMessage(string message) : this(message, new MessageOptions())
        {
        }

        public WarningMessage(string message, MessageOptions options) : base(message, options)
        {
        }

        protected override WarningDisplayPart CreateDisplayPart()
        {
            return new WarningDisplayPart(this);
        }

        protected override void UpdateDisplayOptions(WarningDisplayPart displayPart, MessageOptions options)
        {
            if (options.FontSize != null)
                displayPart.Text.FontSize = options.FontSize.Value;

            displayPart.CloseButton.Visibility = options.ShowCloseButton ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}