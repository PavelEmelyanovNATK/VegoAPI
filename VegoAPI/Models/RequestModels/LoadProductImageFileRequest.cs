using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace VegoAPI.Models.RequestModels
{
    public class LoadProductImageFileRequest
    {
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public IFormFile ImageFile { get; set; }
    }
}
