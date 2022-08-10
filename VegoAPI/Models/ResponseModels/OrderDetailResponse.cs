using System;

namespace VegoAPI.Models.ResponseModels
{
    public class OrderDetailResponse
    {
        public Guid Id { get; set; }
        public DateTime RegistratinDate { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int StatusId { get; set; }
        public int DeliveryTypeId { get; set; }
        public string Comments { get; set; }
    }
}
