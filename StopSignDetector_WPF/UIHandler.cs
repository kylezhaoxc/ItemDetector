using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV;
using System.Windows;
using System.Windows.Media;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace StopSignDetector_WPF
{
    class UIHandler
    {
        public static void show_Image(System.Windows.Controls.Image DEST_UI_TO_SHOW, Image<Bgr, Byte> IMAGE_TO_DISPLAY)
        {
            if (IMAGE_TO_DISPLAY != null)
                DEST_UI_TO_SHOW.Source = ToBitmapSource(IMAGE_TO_DISPLAY.ToBitmap());
        }
        [System.Runtime.InteropServices.DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);
        private static ImageSource ToBitmapSource(System.Drawing.Bitmap image)
        {
            IntPtr ptr = image.GetHbitmap();

            ImageSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                ptr,
                IntPtr.Zero,
                Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(ptr);

            return bs;
        }
    }
}
