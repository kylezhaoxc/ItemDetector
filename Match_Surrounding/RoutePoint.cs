using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Match_Surrounding
{
    class RoutePoint
    {
         
        public Image<Bgr, Byte>[] pointpics;
        public string[] directions;
    }
}
