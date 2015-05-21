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
        int leastPositiveMatch = 5;
        public MainWindow()
        {
            InitializeComponent();
            txt_direction.Content = "Wait for matching";
            txt_dist.Content = null;
            UIHandler.show_Image(modelpic_s, new Image<Bgr, byte>(AppDomain.CurrentDomain.BaseDirectory + "\\images\\no-input-signal.png"));
            global_cap = new Capture(0);
            global_helper = VideoHelper.Ret_Helper(ref global_cap);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
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
                {
                    SpecificItemMatcher.ApplySurfMatching(match_res, video_large, ref cpu, out time, out area, areathreshold, out center);

                    MatchStatus.Enqueue(area);
                    if (MatchStatus.Count > 10) MatchStatus.Dequeue();

                    int PositiveMatchNum = StatusCheck(MatchStatus);
                    if (PositiveMatchNum >= leastPositiveMatch)
                    {
                        Proctime.Content = "Proceed Time:\t" + time.ToString()+"\tms";
                        txt_area.Content = "Matched Area:\t" + area.ToString("f1")+"\tpx^2";
                        signal.Fill = Brushes.Green;
                        MorNM.Content = "Matched";
                        CenterPoints.Enqueue(center);
                        if (CenterPoints.Count >=4) CenterPoints.Dequeue();
                        string Indicator;
                        PositionCheck(CenterPoints, out Indicator);
                        switch (Indicator)
                        {
                            case "Turn Left!":
                                UIHandler.show_Image(direction, new Image<Bgr, byte>(AppDomain.CurrentDomain.BaseDirectory + "\\images\\left.jpg"));
                                txt_direction.Content = "Turn Left!";
                                break;
                            case "Turn Right!":
                                UIHandler.show_Image(direction, new Image<Bgr, byte>(AppDomain.CurrentDomain.BaseDirectory + "\\images\\right.jpg"));
                                txt_direction.Content = "Turn Right!";
                                break;
                            case "Go Straight!":
                                UIHandler.show_Image(direction, new Image<Bgr, byte>(AppDomain.CurrentDomain.BaseDirectory + "\\images\\straight.jpg"));
                                txt_direction.Content = "Go Straight!";
                                break;
                            default:break;
                        }
                        if (area > 100000)
                        {
                            txt_dist.Content = "Getting Close!";
                            if (area > 200000) txt_dist.Content = "Stop!";
                        }
                        else txt_dist.Content = null;
                    }
                    else
                    {
                        signal.Fill = Brushes.Red;
                        MorNM.Content = "No Match";
                        txt_direction.Content = "Wait for matching......";
                        direction.Source = null;
                    }
                }
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
            if (leftvote > 2) Direction = "Turn Left!";
            if (rightvote > 2) Direction = "Turn Right!";
            if (centervote > 2) Direction = "Go Straight!";
        }
        private int StatusCheck(Queue<Double> StatusQueue)
        {
            int positivematch = 0;
            foreach (double area in StatusQueue)
            {
                positivematch += area > areathreshold ? 1 : 0;
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
