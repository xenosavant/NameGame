using System;
using System.Threading.Tasks;

namespace WillowTree.NameGame.Core.Services
{
    public interface IImageService
    {
        Task<byte[]> CropImage(byte[] image);
    }
}
