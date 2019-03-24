using System;

namespace ToastNotifications.Utilities
{
    public interface IDateTimeProvider
    {
        DateTime GetLocalDateTime();
        DateTime GetUtcDateTime();
    }
}