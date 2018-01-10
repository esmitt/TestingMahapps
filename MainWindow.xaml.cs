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
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.Util;
using System.Runtime.InteropServices;
using Emgu.CV.UI;
using System.Drawing;
using System.IO;
// To access MetroWindow, add the following reference
using MahApps.Metro.Controls;

namespace DentalSoft
{
    public static class BitmapSourceConvert
    {
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        public static BitmapSource ToBitmapSource(IImage image)
        {
            using (System.Drawing.Bitmap source = image.Bitmap)
            {
                IntPtr ptr = source.GetHbitmap();

                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(ptr);
                return bs;
            }
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private ImageSource ConvertBitmapToImageSource(Bitmap imToConvert)
        {
            Bitmap bmp = new Bitmap(imToConvert);
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            ImageSource sc = (ImageSource)image;

            return sc;
        }

        private void Grid_Initialized(object sender, EventArgs e)
        {
            try
            {
                //this works
                //Mat theMat = new Mat("image.jpg");
                //canvasImg.Source = ConvertBitmapToImageSource(theMat.Bitmap);

                //also this works
                //Image<Rgb, Byte> image = new Image<Rgb, byte>("image.jpg");
                //Image<Gray, Byte> p = image.Convert<Gray, byte>();
                //canvasImg.Source = BitmapSourceConvert.ToBitmapSource(p);

                //and this works too
                Image<Rgb, Byte> image = new Image<Rgb, byte>("image.jpg");
                Image<Gray, Byte> p = image.Convert<Gray, byte>();
                canvasImg.Source = ConvertBitmapToImageSource(p.Bitmap);
            }
            catch (Exception ev)
            {
                MessageBox.Show(ev.Message);
            }
            //Mat img = CvInvoke.Imread("myimage.jpg", ImreadModes.AnyColor);
            //canvasImg.Source = img.Bitmap;
        }
    }
}
