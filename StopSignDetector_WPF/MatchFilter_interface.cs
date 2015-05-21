using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace StopSignDetector_WPF
{
    interface MatchFilter_interface
    {
        int CountContours(System.Drawing.Bitmap temp);
        double Getarea(PointF[] pts);
    }
}
