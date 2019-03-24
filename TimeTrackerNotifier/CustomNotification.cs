
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ToastNotifications.Core;

namespace TimeTracker.Notifications
{
    public class CustomNotification : NotificationBase, INotifyPropertyChanged
    {
        private CustomDisplayPart _displayPart;

        public override NotificationDisplayPart DisplayPart => _displayPart ?? (_displayPart = new CustomDisplayPart(this));

        public CustomNotification(string title, string message)
        {
            Title = title;
            Text = message;
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
