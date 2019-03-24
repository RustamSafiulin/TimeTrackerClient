
using System;

namespace TimeTracker.EventBus
{
    public class NotificationMessage : ITinyMessage
    {
        public String Text { get; set; }

        public DateTime TimeStamp { get; set; }

        public object Sender { get; private set; }
    }
}
