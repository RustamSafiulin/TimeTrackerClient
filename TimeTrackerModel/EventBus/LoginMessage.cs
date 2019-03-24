
using System;

using TimeTracker.Models;

namespace TimeTracker.EventBus
{
    public class LoginMessage : ITinyMessage
    {
        public SessionInfoDto SessionInfo { get; set; }

        public object Sender { get; private set; }
    }
}
