using ImageMagick;
using System.IO;
using System.Threading.Tasks;

namespace IconConverter
{
    public static class ImageOperations
    {
        internal static async Task SaveWithoutCropAsync(string fileName, int dimension)
        {
            await Task.Run(() =>
            {
                string folderName = Path.GetFileNameWithoutExtension(fileName);
                Directory.CreateDirectory(folderName);

                using (MagickImage img = new MagickImage(fileName))
                {
                    ResizeAndSave(img, dimension, Path.Combine(folderName, $"WholeIcon_{dimension}.ico"));
                }
            });
        }

        internal static void ResizeAndSave(MagickImage image, int dimension, string outputPath)
        {
            using (MagickImage resized = new MagickImage(image))
            {
                resized.Resize(new MagickGeometry(dimension, dimension) { IgnoreAspectRatio = true });
                resized.Write(outputPath);
            }
        }

        internal static async Task ResizeAndSaveAsync(MagickImage image, int dimension, string outputPath)
        {
            await Task.Run(() =>
            {
                // We clone the image inside the task to be thread-safe if 'image' is shared,
                // or we rely on the caller passing a thread-safe object.
                // Since MagickImage is not thread-safe for write operations if shared,
                // but here we are creating a new 'resized' instance.
                // However, 'image' source might be accessed.
                // For safety with the Magick.NET wrapper, let's clone.
                using (MagickImage resized = new MagickImage(image))
                {
                    resized.Resize(new MagickGeometry(dimension, dimension) { IgnoreAspectRatio = true });
                    resized.Write(outputPath);
                }
            });
        }

        internal static async Task MergeIconsAsync(string[] inputFiles, string outputFile)
        {
            await Task.Run(() =>
            {
                using (MagickImageCollection collection = new MagickImageCollection())
                {
                    foreach (string file in inputFiles)
                    {
                        collection.Add(file);
                    }
                    collection.Write(outputFile);
                }
            });
        }
    }
}
