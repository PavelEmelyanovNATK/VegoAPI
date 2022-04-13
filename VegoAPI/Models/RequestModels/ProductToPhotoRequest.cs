using System;

namespace VegoAPI.Models.RequestModels
{
    public class ProductToPhotoRequest
    {
        public Guid ProductId { get; set; }
        public Guid PhotoId { get; set; }
    }
}
