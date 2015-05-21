using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.Features2D;
using System.IO;


namespace StopSignDetector_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        Queue<double> MatchStatus = new Queue<double>();
        Queue<System.Drawing.Point> CenterPoints = new Queue<System.Drawing.Point>();
        private Capture global_cap;
        private VideoHelper global_helper;
        private Image<Bgr, Byte> video_small, video_large, model_small;
        private Image<Bgr, Byte> model_pic;
        SurfProcessor cpu = new SurfProcessor();
        long time;double area;  int areathreshold = 500;
        System.Drawing.Point center;
        int leastPositiveMatch = 50;
        public MainWindow()
        {
            InitializeComponent();
            global_cap = new Capture(0);
            global_helper = VideoHelper.Ret_Helper(ref global_cap);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(40);
            timer.Tick += new EventHandler(Periodical_Tick_Handler);
            timer.Start();
        }
        public void Periodical_Tick_Handler(object sender, EventArgs e)
        {
           
                global_helper.Ret_Frames(out video_small, out video_large);
                if (video_small != null)
                {
                    UIHandler.show_Image(videoframe_s, video_small);
                if (video_large != null && model_pic != null)
                    SpecificItemMatcher.ApplySurfMatching(match_res, video_large, ref cpu, out time, out area, areathreshold, out center);
                /*MatchStatus.Enqueue(area);
                if (MatchStatus.Count > 100) MatchStatus.Dequeue();
                int PositiveMatchNum= StatusCheck(MatchStatus);
                if (PositiveMatchNum >= leastPositiveMatch)
                {
                    CenterPoints.Enqueue(center);
                    if (CenterPoints.Count > 200) CenterPoints.Dequeue();
                    string Indicator;
                    PositionCheck(CenterPoints, out Indicator);
                }*/
            }
            
        }
        public void PositionCheck(Queue<System.Drawing.Point> CenterPointsQueue,out string Direction)
        {
            int xmax = 420;
            int xmin = 220;
            Direction = "N/A";
            int leftvote = 0, rightvote = 0, centervote = 0;
            foreach (System.Drawing.Point center in CenterPointsQueue)
            {
                if (center.X > xmax) rightvote++;
                if (center.X < xmin) leftvote++;
                if (center.X > xmin && center.X < xmax) centervote++;
            }
            if (leftvote > 100) Direction = "Turn Left!";
            if (rightvote > 100) Direction = "Turn Right!";
            if (centervote > 100) Direction = "Go Straight!";
        }
        private int StatusCheck(Queue<Double> StatusQueue)
        {
            int positivematch = 0; double sum = 0; int count = 0;
            foreach (double area in StatusQueue)
            {
                positivematch += area > areathreshold ? 1 : 0;
                sum += area; count++;
            }

            return positivematch;
        }



        private void shot_Click(object sender, RoutedEventArgs e)
        {
            model_small = video_small;
            UIHandler.show_Image(modelpic_s, model_small);
            model_pic = video_large;
            model_pic.Save(AppDomain.CurrentDomain.BaseDirectory+"\\modelpicture.jpg");

        }

      

       

       
       
    }
}
