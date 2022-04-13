using System;

namespace VegoAPI.Models.ResponseModels
{
    public class ProductDetailResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public PhotoResponse[] Photos { get; set; }
        public Guid? MainPhotoId { get; set; }
    }
}
