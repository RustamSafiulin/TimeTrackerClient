
using System;

namespace TimeTracker.EventBus
{
    public class NeedServerSyncMessage : ITinyMessage
    {
        public object Sender { get; private set; }
    }
}
