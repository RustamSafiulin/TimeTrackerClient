using System.Windows.Input;

namespace ToastNotifications.Events
{
    public class BlockAllKeyInputEventHandler: IKeyboardEventHandler
    {
        public void Handle(KeyEventArgs eventArgs)
        {
            eventArgs.Handled = true;
        }
    }
}