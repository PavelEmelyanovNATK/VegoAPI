using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VegoAPI.Models.RequestModels
{
    public class EditEntityWithGuidRequest
    {
        [Required]
        public Guid EntityId { get; set; }
        [Required]
        public Dictionary<string, string> ChangedFields { get; set; }
    }
}
