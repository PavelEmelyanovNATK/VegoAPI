namespace VegoAPI.Models.RequestModels
{
    public class FilteredProductsRequest
    {
        public int[] CategoriesIds { get; set; }
        public string Filter { get; set; }
        public int PagesCount { get; set; } = 1;
    }
}
