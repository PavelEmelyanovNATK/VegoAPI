using System.ComponentModel.DataAnnotations;

namespace VegoAPI.Models.RequestModels
{
    public class AddProductTypeRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
