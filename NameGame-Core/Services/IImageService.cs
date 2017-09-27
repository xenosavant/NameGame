// This interface represents an abstraction for a platform-specific image service
// That will crop an image in byte array format and return a new byte array.

using System;
using System.Threading.Tasks;

namespace WillowTree.NameGame.Core.Services
{
    public interface IImageService
    {
        Task<byte[]> CropImage(byte[] image);
    }
}
