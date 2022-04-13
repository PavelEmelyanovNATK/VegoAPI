using System;

namespace VegoAPI.Models.ResponseModels
{
    public class PhotoResponse
    {
        public Guid PhotoId { get; set; }
        public string LowResPath { get; set; }
        public string HighResPath { get; set; }
    }
}
