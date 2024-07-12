using IconConverter.IconEx;
using ImageMagick;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;

namespace IconConverter
{
    public static class ImageOperations
    {

        enum Direction
        {
            NE, SE, SW, NW
        }

        private static Direction getDirection(System.Windows.Shapes.Rectangle rect)
        {
            if (rect.VerticalAlignment == VerticalAlignment.Top && rect.HorizontalAlignment == HorizontalAlignment.Left)
                return Direction.SE;
            if (rect.VerticalAlignment == VerticalAlignment.Top && rect.HorizontalAlignment == HorizontalAlignment.Right)
                return Direction.SW;
            if (rect.VerticalAlignment == VerticalAlignment.Bottom && rect.HorizontalAlignment == HorizontalAlignment.Left)
                return Direction.NE;

            return Direction.NW;
        }

        internal static void CropAndSave(MagickImage img, string fileName, int dimension, System.Windows.Shapes.Rectangle rect, double scale, System.Windows.Controls.Image pictureBox)
        {
            Directory.CreateDirectory(Path.GetFileNameWithoutExtension(fileName));

            img = new MagickImage(img);

            cropImage(img, rect, scale, pictureBox);

            string iconFilename = Path.Combine(Path.GetFileNameWithoutExtension(fileName), $"CroppedIcon_{dimension}.ico");

            img.Scale(new MagickGeometry(dimension) { IgnoreAspectRatio = true });
            Console.WriteLine(img.HasAlpha + " " + img.Settings.Depth);

            if (dimension == 256)
            {
                if ((img.Format == MagickFormat.Jpeg || img.Format == MagickFormat.Jpg) && MessageBox.Show("Compact into jpg .ico?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    IconFileWriter x = new IconFileWriter();
                    img.HasAlpha = true;
                    x.Images.Add(ToBitmap(img));
                    x.Save(iconFilename);
                    return;
                }
                else
                {
                    img.HasAlpha = true;
                }
            }

            img.Write(iconFilename);
        }

        private static void cropImage(MagickImage img, System.Windows.Shapes.Rectangle rect, double scale, System.Windows.Controls.Image pictureBox)
        {
            int width = (int)(rect.Width / scale);
            int height = (int)(rect.Height / scale);

            int left = 0;
            int top = 0;

            switch (getDirection(rect))
            {
                case Direction.NE:
                    left = (int)(rect.Margin.Left / scale);
                    top = (int)((pictureBox.Height - (rect.Margin.Bottom + rect.Height)) / scale);
                    break;
                case Direction.SE:
                    left = (int)(rect.Margin.Left / scale);
                    top = (int)(rect.Margin.Top / scale);
                    break;
                case Direction.SW:
                    left = (int)((pictureBox.Width - rect.Margin.Right - rect.Width) / scale);
                    top = (int)(rect.Margin.Top / scale);
                    break;
                case Direction.NW:
                    left = (int)((pictureBox.Width - (rect.Margin.Right + rect.Width)) / scale);
                    top = (int)((pictureBox.Height - (rect.Margin.Bottom + rect.Height)) / scale);
                    break;
            }

            MagickGeometry geometry = new MagickGeometry(left, top, width, height);
            img.Crop(geometry);
        }

        internal static void SaveWithoutCrop(string fileName, int dimension)
        {
            Directory.CreateDirectory(Path.GetFileNameWithoutExtension(fileName));

            MagickImage img = new MagickImage(fileName);
            img.Scale(new MagickGeometry(dimension) { IgnoreAspectRatio = true });
            Bitmap bmp = ToBitmap(img);
            byte[] bmpBytes = BitmapToBytes(bmp);  // Convert Bitmap to byte[]
            MagickImage final = new MagickImage(bmpBytes);  // Use byte[] to create MagickImage

            string iconFilename = Path.Combine(Path.GetFileNameWithoutExtension(fileName), $"WholeIcon_{dimension}.ico");

            final.Write(iconFilename);
        }

        internal static IconDeviceImage GetIconBMP(string fileName)
        {
            MagickImage ico = new MagickImage(fileName);
            IconDeviceImage IconDeviceImage = new IconDeviceImage(new System.Drawing.Size(ico.Width, ico.Height), System.Windows.Forms.ColorDepth.Depth32Bit);
            IconDeviceImage.IconImage = ToBitmap(ico);

            return IconDeviceImage;
        }

        internal static Image GetIconPNG(string fileName)
        {
            MagickImage ico = new MagickImage(fileName);
            return ToBitmap(ico);
        }

        private static Bitmap ToBitmap(MagickImage image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Write(memoryStream, MagickFormat.Png);
                memoryStream.Position = 0;
                return new Bitmap(memoryStream);
            }
        }

        private static byte[] BitmapToBytes(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
