
using System;

namespace TimeTracker.EventBus
{
    public class UpdateNotifyBadgeMessage : ITinyMessage
    {
        public int Value { get; set; }
        public object Sender { get; set; }
    }
}
