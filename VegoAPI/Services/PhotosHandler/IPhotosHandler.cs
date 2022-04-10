using System;
using System.Threading.Tasks;
using VegoAPI.Models.RequestModels;

namespace VegoAPI.Services.PhotosHandler
{
    public interface IPhotosHandler
    {
        Task<(string, string)> SaveProductPhoto(LoadProductImageRequest loadProductImageRequest);
        Task DeleteProductPhoto(string path);
    }
}
