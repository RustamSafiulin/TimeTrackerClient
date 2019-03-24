
using System;

using TimeTracker.Models;

namespace TimeTracker.EventBus
{
    public class SignupMessage : ITinyMessage
    {
        public SuccessMsgDto SuccessMsg { get; set; }

        public object Sender { get; private set; }
    }
}
