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
    class SpecificItemMatcher
    {
        public static void ApplySurfMatching(System.Windows.Controls.Image UI_TO_SHOW_THE_IMAGE, Image<Bgr, Byte> OBSERVED_IMAGE,ref SurfProcessor PROCESSOR_TO_USE, out long TIME_USED, out double MATCHED_AREA, int MIN_ACCEPTABLE_MATCHED_AREA, out System.Drawing.Point CENTER_OF_MATCHED_REGION)
        {
            Image<Gray, Byte> m_g = new Image<Gray, Byte>(AppDomain.CurrentDomain.BaseDirectory + "\\modelpicture.jpg");
            System.Drawing.Bitmap v1 = OBSERVED_IMAGE.ToBitmap();
            Image<Gray, Byte> v_g = new Image<Gray, Byte>(v1);
            Image<Bgr, Byte> match_result = PROCESSOR_TO_USE.DrawResult(m_g, v_g, out TIME_USED, out MATCHED_AREA, MIN_ACCEPTABLE_MATCHED_AREA, out CENTER_OF_MATCHED_REGION);

            UIHandler.show_Image(UI_TO_SHOW_THE_IMAGE, match_result);
        }
    }
}
