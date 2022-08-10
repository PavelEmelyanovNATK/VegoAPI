using System;

namespace VegoAPI.Models.RequestModels
{
    public class ChangeOrderStatusRequest
    {
        public Guid OrderId { get; set; }
        public int StatusId { get; set; }
    }
}
