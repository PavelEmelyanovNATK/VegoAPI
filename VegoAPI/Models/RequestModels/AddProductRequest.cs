using System.ComponentModel.DataAnnotations;

namespace VegoAPI.Models.RequestModels
{
    public class AddProductRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public int ProductTypeId { get; set; }
        [Required]
        public double Price { get; set; }
        public string Description { get; set; }
    }
}
