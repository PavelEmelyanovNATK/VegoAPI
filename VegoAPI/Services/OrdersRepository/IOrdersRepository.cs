using System;
using System.Threading.Tasks;
using VegoAPI.Models.RequestModels;
using VegoAPI.Models.ResponseModels;

namespace VegoAPI.Services.OrdersRepository
{
    public interface IOrdersRepository
    {
        Task<OrderShortResponse[]> GetAllOrdersAsync(int page, int ordersPerPage);

        Task<OrderDetailResponse> GetOrderByIdAsync(Guid id);

        Task AddOrderAsync(AddOrderRequest addOrderRequest);

        Task ChangeOrderStatusAsync(ChangeOrderStatusRequest changeOrderStatusRequest);
    }
}
