using Microsoft.AspNetCore.Hosting;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading.Tasks;
using VegoAPI.Models.RequestModels;
using VegoAPI.Services.ProductsRepository;

namespace VegoAPI.Services.PhotosHandler
{
    public class PhotosHandler : IPhotosHandler
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PhotosHandler(
            IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public Task DeleteProductPhoto(string path)
        {
            File.Delete(path);

            return Task.CompletedTask;
        }

        public async Task<(string, string)> SaveProductPhoto(LoadProductImageFileRequest loadProductImageRequest)
        {
            using var memoryStream = new MemoryStream();
            await loadProductImageRequest.ImageFile.CopyToAsync(memoryStream);
            using var sourceImage = Bitmap.FromStream(memoryStream);

            float scale = 0;

            if (sourceImage.Height > sourceImage.Width)
                if (sourceImage.Width <= 160)
                    scale = 1;
                else
                    scale = sourceImage.Width / 160f;
            else
                if (sourceImage.Height <= 80)
                scale = 1;
            else
                scale = sourceImage.Height / 80f;

            var lowResImage = new Bitmap((int)(sourceImage.Width / scale), (int)(sourceImage.Height / scale));

            using (Graphics gr = Graphics.FromImage(lowResImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(sourceImage, new Rectangle(0, 0, (int)(sourceImage.Width / scale), (int)(sourceImage.Height / scale)));
            }

            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "photos");
            var path1 = Path.Combine(_webHostEnvironment.ContentRootPath, "photos", Guid.NewGuid().ToString() + ".jpg");
            var path2 = Path.Combine(_webHostEnvironment.ContentRootPath, "photos", Guid.NewGuid().ToString() + ".jpg");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            lowResImage.Save(path1);
            sourceImage.Save(path2);

            return (path1, path2);
        }
    }
}
