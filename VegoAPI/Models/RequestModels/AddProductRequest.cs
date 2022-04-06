namespace VegoAPI.Models.RequestModels
{
    public class AddProductRequest
    {
        public string Title { get; set; }
        public int ProductTypeId { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
    }
}
