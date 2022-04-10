using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VegoAPI.Models.RequestModels
{
    public class LoadProductImageRequest
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public IFormFile ImageFile { get; set; }
    }
}
