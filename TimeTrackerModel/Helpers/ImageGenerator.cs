
using System;
using System.Windows;
using System.Windows.Data;

using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Globalization;

using System.IO;

namespace TimeTracker.Helpers
{
    public static class ImageGenerator
    {
        public static byte[] GenerateEmptyAvatarImage(string initials)
        {
            var visual = new DrawingVisual();

            using (var drawingContext = visual.RenderOpen())
            {
                //#2f889a
                var bc = new BrushConverter();

                drawingContext.DrawEllipse((Brush)bc.ConvertFrom("#2f889a"), new System.Windows.Media.Pen(System.Windows.Media.Brushes.Black, 0), new System.Windows.Point(45, 45), 45, 45);
                drawingContext.DrawText(
                    new FormattedText(initials, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                        new Typeface("Segoe UI"), 32, System.Windows.Media.Brushes.LightGray), new System.Windows.Point(28, 24));
            }

            var width = 90;
            var height = 90;
            var bitmap = new RenderTargetBitmap((int)width, (int)height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(visual);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            byte[] result;
            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                result = ms.ToArray();
            }

            return result;
        }
    }
}
