using System.Windows;

namespace ToastNotifications.Position
{
    public static class PositionExtensions
    {
        public static  Point GetActualPosition(this UIElement element)
        {
            var pt = element.PointToScreen(new Point(0, 0));
			var source = PresentationSource.FromVisual(element);
			return source.CompositionTarget.TransformFromDevice.Transform(pt);
		}
    }
}