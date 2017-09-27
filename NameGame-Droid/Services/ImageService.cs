// This implementation of the IImageService crops the image using an 
// Android.Graphics canvas

using System;
using Android.Graphics;
using WillowTree.NameGame.Core.Services;
using System.IO;
using System.Threading.Tasks;

namespace WillowTree.NameGame.Droid.Services
{
    public class ImageService : IImageService
    {
        public async Task<byte[]> CropImage(byte[] image)
        {
            // Decode the byte array to a bitmap for processing
            var bitmap = BitmapFactory.DecodeByteArray(image, 0, image.Length);
            var width = (int)bitmap.Width;
            var height = (int)bitmap.Height;
            Rect dstRect;
            Rect srcRect;
            int sides;
            // If the image has a landscape aspect ratio, crop out the middle
            if (width > height)
            {
                sides = height;
                dstRect = new Rect(0, 0, sides, sides);
                srcRect = new Rect((width - height) / 2, 0, ((width - height) / 2) + height, height);
            }
            // If the image has a portrait aspect ratio, crop out a square from the top
            else
            {
                sides = width;
                dstRect = new Rect(0, 0, sides, sides);
                srcRect = new Rect(0, 0, sides, sides);
            }

            // crop out the selected rectangle, compress to a JPEG stream, and return the byte array
            using (Bitmap croppedImage = Bitmap.CreateBitmap(sides, sides, Bitmap.Config.Rgb565))
            using (MemoryStream stream = new MemoryStream())
            {
                Canvas canvas = new Canvas(croppedImage);
                canvas.DrawBitmap(bitmap, srcRect, dstRect, null);
                await croppedImage.CompressAsync(Bitmap.CompressFormat.Jpeg, 100, stream);
                return stream.ToArray();
            }
        }
    }
}
