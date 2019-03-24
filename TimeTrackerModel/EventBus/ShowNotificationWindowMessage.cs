
using System;

namespace TimeTracker.EventBus
{
    public class ShowNotificationWindowMessage : ITinyMessage
    {
        public object Sender { get; private set; }
    }
}
