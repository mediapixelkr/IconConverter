using ImageMagick;
using System.IO;

namespace IconConverter
{
    public static class ImageOperations
    {
        internal static void SaveWithoutCrop(string fileName, int dimension)
        {
            string folderName = Path.GetFileNameWithoutExtension(fileName);
            Directory.CreateDirectory(folderName);

            using (MagickImage img = new MagickImage(fileName))
            {
                ResizeAndSave(img, dimension, Path.Combine(folderName, $"WholeIcon_{dimension}.ico"));
            }
        }

        internal static void ResizeAndSave(MagickImage image, int dimension, string outputPath)
        {
            using (MagickImage resized = new MagickImage(image))
            {
                resized.Resize(new MagickGeometry(dimension, dimension) { IgnoreAspectRatio = true });
                resized.Write(outputPath);
            }
        }

        internal static void MergeIcons(string[] inputFiles, string outputFile)
        {
            using (MagickImageCollection collection = new MagickImageCollection())
            {
                foreach (string file in inputFiles)
                {
                    collection.Add(file);
                }
                collection.Write(outputFile);
            }
        }
    }
}
