
using System;

namespace TimeTracker.EventBus
{
    public class LogoutMessage : ITinyMessage
    {
        public object Sender { get; private set; }
    }
}
