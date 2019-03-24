using System;
using System.Windows;

namespace ToastNotifications.Core
{
    public interface IPositionProvider : IDisposable
    {
        Window ParentWindow { get; }
        Point GetPosition(double actualPopupWidth, double actualPopupHeight);
        double GetHeight();
        EjectDirection EjectDirection { get; }

        event EventHandler UpdatePositionRequested;
        event EventHandler UpdateEjectDirectionRequested;
        event EventHandler UpdateHeightRequested;
    }
}