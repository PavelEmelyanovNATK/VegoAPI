using System;
using System.ComponentModel.DataAnnotations;

namespace VegoAPI.Models.RequestModels
{
    public class AddProductPhotoRequest
    {
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public string LowImagePath { get; set; }
        [Required]
        public string HighImagePath { get; set; }
    }
}
