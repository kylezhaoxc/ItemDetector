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
        public static void TellDirection(System.Windows.Controls.Image DEST_IMG_TO_SHOW,System.Windows.Controls.Label DEST_TXT_TO_SHOW,string DIRECTION_TXT)
        {
            switch (DIRECTION_TXT)
            {
                case "Turn Left!":
                    UIHandler.show_Image(DEST_IMG_TO_SHOW, new Image<Bgr, byte>(AppDomain.CurrentDomain.BaseDirectory + "\\images\\left.jpg"));
                    DEST_TXT_TO_SHOW.Content = "Turn Left!";
                    break;
                case "Turn Right!":
                    UIHandler.show_Image(DEST_IMG_TO_SHOW, new Image<Bgr, byte>(AppDomain.CurrentDomain.BaseDirectory + "\\images\\right.jpg"));
                    DEST_TXT_TO_SHOW.Content = "Turn Right!";
                    break;
                case "Go Straight!":
                    UIHandler.show_Image(DEST_IMG_TO_SHOW, new Image<Bgr, byte>(AppDomain.CurrentDomain.BaseDirectory + "\\images\\straight.jpg"));
                    DEST_TXT_TO_SHOW.Content = "Go Straight!";
                    break;
                default: break;
            }
        }
    }
}
