namespace VegoAPI.Models.RequestModels
{
    public class AddOrderRequest
    {
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int StatusId { get; set; }
        public int DeliveryTypeId { get; set; }
        public string Comments { get; set; }
    }
}
