
using System;

using TimeTracker.Helpers;

namespace TimeTracker.Models
{
    public class NotificationDataModel : DomainModelBase
    {
        private String _notificationMessage;
        public String NotificationMessage
        {
            get { return _notificationMessage; }
            set { SetProperty(ref _notificationMessage, value); }
        }

        private Boolean _isReaded;
        public Boolean IsReaded
        {
            get { return _isReaded; }
            set { SetProperty(ref _isReaded, value); }
        }

        private DateTime _timeStamp;
        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set { SetProperty(ref _timeStamp, value); }
        }

        public NotificationDataModel()
        { }
    }
}
