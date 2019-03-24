namespace ToastNotifications.Core
{
    public interface INotificationAnimator
    {
        void Setup();
        void PlayShowAnimation();
        void PlayHideAnimation();
    }
}