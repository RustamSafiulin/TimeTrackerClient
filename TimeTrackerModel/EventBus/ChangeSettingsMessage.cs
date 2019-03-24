
using System;
using System.Collections.Generic;

namespace TimeTracker.EventBus
{
    public class ChangeSettingsMessage : ITinyMessage
    {
        public object Sender { get; private set; }

        public Boolean NeedBegin { get; set; }

        public Boolean NeedPopupNotify { get; set; }

        public Boolean NeedSoundNotify { get; set; }

        public List<String> Categories { get; set; }
    }
}
