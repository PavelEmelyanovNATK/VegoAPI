using System;

namespace VegoAPI.Models.ResponseModels
{
    public class OrderShortResponse
    {
        public Guid Id { get; set; }
        public DateTime RegistratinDate { get; set; }
        public string ClientName { get; set; }
        public string Status { get; set; }
    }
}
