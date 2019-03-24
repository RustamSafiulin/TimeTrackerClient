
using System;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace TimeTracker
{
    public class EnterKeyDownEventTrigger : System.Windows.Interactivity.EventTrigger
    {
        public EnterKeyDownEventTrigger() : base("KeyDown")
        {}

        protected override void OnEvent(EventArgs eventArgs)
        {
            var e = eventArgs as KeyEventArgs;
            if (e != null && e.Key == Key.Enter)
                this.InvokeActions(eventArgs);
        }
    }
}