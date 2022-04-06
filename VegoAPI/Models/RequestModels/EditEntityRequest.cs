using System.Collections.Generic;

namespace VegoAPI.Models.RequestModels
{
    public class EditEntityRequest
    {
        public int EntityId { get; set; }
        public Dictionary<string, string> ChangedFields { get; set; }
    }
}
