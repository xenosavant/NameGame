using System;
using Android.Graphics;
using Mono;
using Java.Net;
using WillowTree.NameGame.Core.Services;
using System.IO;
using System.Threading.Tasks;

namespace WillowTree.NameGame.Droid.Services
{
    public class ImageService : IImageService
    {
        public async Task<byte[]> CropImage(byte[] image)
        {
            var bitmap = BitmapFactory.DecodeByteArray(image, 0, image.Length);
            var width = (int)bitmap.Width;
            var height = (int)bitmap.Height;
            Rect dstRect;
            Rect srcRect;
            int sides;
            if (width > height)
            {
                sides = height;
                dstRect = new Rect(0, 0, sides, sides);
                srcRect = new Rect((width - height) / 2, 0, ((width - height) / 2) + height, height);
            }
            else {
                sides = width;
                dstRect = new Rect(0, 0, sides, sides);
                srcRect = new Rect(0, 0, sides, sides);
            }

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
