using System.Windows;
using ToastNotifications.Core;
using ToastNotifications.Messages.Core;

namespace ToastNotifications.Messages.Information
{
    public class InformationMessage : MessageBase<InformationDisplayPart>
    {
        public InformationMessage(string message) : this(message, new MessageOptions())
        {
        }

        public InformationMessage(string message, MessageOptions options) : base(message, options)
        {
        }

        protected override InformationDisplayPart CreateDisplayPart()
        {
            return new InformationDisplayPart(this);
        }

        protected override void UpdateDisplayOptions(InformationDisplayPart displayPart, MessageOptions options)
        {
            if (options.FontSize != null)
                displayPart.Text.FontSize = options.FontSize.Value;

            displayPart.CloseButton.Visibility = options.ShowCloseButton ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}