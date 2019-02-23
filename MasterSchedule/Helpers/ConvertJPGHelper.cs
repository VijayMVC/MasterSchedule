using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;

namespace MasterSchedule.Helpers
{
    public class ConvertJPGHelper
    {
        public static byte[] GetJPGFromBitmapImage(BitmapImage bitmapImage)
        {
            MemoryStream memoryStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            encoder.Save(memoryStream);
            return memoryStream.ToArray();
        }

        public static BitmapImage GetBitmapImageFromJPG(byte[] jpg)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(jpg);
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return bitmapImage;
        }
    }
}
