
using System;

namespace TimeTracker.EventBus
{
    public class AppExitMessage : ITinyMessage
    {
        public object Sender { get; private set; }
    }
}
