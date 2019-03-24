
using System.Windows.Input;

namespace ToastNotifications.Events
{
    public interface IKeyboardEventHandler
    {
        void Handle(KeyEventArgs eventArgs);
    }
}
