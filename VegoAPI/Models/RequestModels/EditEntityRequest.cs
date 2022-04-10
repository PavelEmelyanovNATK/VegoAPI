using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VegoAPI.Models.RequestModels
{
    public class EditEntityRequest
    {
        [Required]
        public int EntityId { get; set; }
        [Required]
        public Dictionary<string, string> ChangedFields { get; set; }
    }
}
