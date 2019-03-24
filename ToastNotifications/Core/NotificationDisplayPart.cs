using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToastNotifications.Display;
using ToastNotifications.Utilities;

namespace ToastNotifications.Core
{
    public abstract class NotificationDisplayPart : UserControl
    {
        protected INotificationAnimator Animator;
        public INotification Notification { get; protected set; }

        protected NotificationDisplayPart()
        {
            Animator = new NotificationAnimator(this, TimeSpan.FromMilliseconds(300), TimeSpan.FromMilliseconds(300));

            Margin = new Thickness(1);

            Animator.Setup();

            Loaded += OnLoaded;
            MinHeight = 60;
        }

        public virtual MessageOptions GetOptions()
        {
            return null;
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            var dc = DataContext as INotification;
            var opts = dc?.DisplayPart?.GetOptions();
            if (opts != null && opts.FreezeOnMouseEnter)
            {
                if (!opts.UnfreezeOnMouseLeave) // message stay freezed, show close button
                {
                    var bord2 = this.Content as Border;
                    if (bord2 != null)
                    {
                        if (dc.CanClose)
                        {
                            dc.CanClose = false;
                            var btn = this.FindChild<Button>("CloseButton");
                            btn.Visibility = Visibility.Visible;
                        }
                    }
                }
                else
                {
                    dc.CanClose = false;
                }
            }
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            var dc = DataContext as INotification;
            var opts = dc?.DisplayPart?.GetOptions();
            if (opts != null && opts.FreezeOnMouseEnter && opts.UnfreezeOnMouseLeave)
            {
                dc.CanClose = true;
            }
            base.OnMouseLeave(e);
        }


        public virtual string GetMessage()
        {
            return "?";
        }

        public void Bind<TNotification>(TNotification notification) where TNotification : INotification
        {
            Notification = notification;
            DataContext = Notification;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Animator.PlayShowAnimation();
        }

        public void OnClose()
        {
            Animator.PlayHideAnimation();
        }
    }
}
