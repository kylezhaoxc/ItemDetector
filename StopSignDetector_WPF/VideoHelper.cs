using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.ML;
using Emgu.CV.CvEnum;

namespace StopSignDetector_WPF
{
    class VideoHelper { 
        private  static VideoHelper helper;
        private  Capture cap;   
        public static VideoHelper Ret_Helper(ref Capture capture)
        {
            if (helper == null)
            {
                helper = new VideoHelper();
                helper.cap = capture;
                return helper;
            }
            else return helper;
        }

        public void Ret_Frames(out Image<Bgr,Byte>SMALL_SCALE_FRAME, out Image<Bgr, Byte> LARGE_SCALE_FRAME)
        {
            SMALL_SCALE_FRAME = null; LARGE_SCALE_FRAME = null;
            while (cap.Grab()) { LARGE_SCALE_FRAME = cap.QueryFrame(); SMALL_SCALE_FRAME = cap.QuerySmallFrame();return; }
        }
      
    }
}
