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
    class CamHelper { 
        private  static CamHelper helper;
        private  Capture cap;
        private int capcount = 0;
        public static CamHelper Ret_Helper(ref Capture capture)
        {
            if (helper == null)
            {
                helper = new CamHelper();
                helper.cap = capture;
                return helper;
            }
            else return helper;
        }

        public void Ret_Frames(out Image<Bgr,Byte>small, out Image<Bgr, Byte> large)
        {
            small = null;large = null;
            if (cap.Grab()) { large=cap.QueryFrame();small= cap.QuerySmallFrame();return; }
        }
      
    }
}
