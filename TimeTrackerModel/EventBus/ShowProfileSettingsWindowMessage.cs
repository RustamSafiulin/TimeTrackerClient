
using System;

namespace TimeTracker.EventBus
{
    public class ShowProfileSettingsWindowMessage : ITinyMessage
    {
        public object Sender { get; private set; }
    }
}
