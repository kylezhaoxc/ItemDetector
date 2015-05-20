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
        private Capture global_cap;
        private CamHelper global_helper;
        private Image<Bgr, Byte> video_small, video_large, model_small, model_large;
        private bool model_selected = false;
        public MainWindow()
        {
            InitializeComponent();
            global_cap = new Capture(0);
            global_helper = CamHelper.Ret_Helper(ref global_cap);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(40);
            timer.Tick += new EventHandler(take_picture_and_analyze);
            timer.Start();
        }
        public void take_picture_and_analyze(object sender, EventArgs e)
        {
            global_helper.Ret_Frames(out video_small,out video_large);
            if (video_small != null)
            {
                show_Image(videoframe_s, video_small);
            }
        }
        [System.Runtime.InteropServices.DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);
        public static ImageSource ToBitmapSource(System.Drawing.Bitmap image)
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
        public void show_Image(System.Windows.Controls.Image imgbox, Image<Bgr, Byte> img)
        {
            imgbox.Source = ToBitmapSource(img.ToBitmap());
        }
        private void shot_Click(object sender, RoutedEventArgs e)
        {
            model_large = video_large;
            model_small = video_small;
            show_Image(modelpic_s, model_small);
            if (model_large != null) model_selected = true;
        }
    }
}
