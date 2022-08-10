using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using VegoAPI.Models.RequestModels;
using VegoAPI.Models.ResponseModels;
using VegoAPI.VegoAPI.Models.DBEntities;
using VegoAPI.VegoAPI.Services.DBContext;

namespace VegoAPI.Services.OrdersRepository
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly VegoCityServerDBContext _dao;

        public OrdersRepository(VegoCityServerDBContext dao)
        {
            _dao = dao;
        }

        public async Task AddOrderAsync(AddOrderRequest addOrderRequest)
        {
            var registrationDate = DateTime.Now;
            var order = new Order
            {
                Id = Guid.NewGuid(),
                DeliveryTypeId = addOrderRequest.DeliveryTypeId,
                StatusId = 1,
                ClientName = addOrderRequest.ClientName,
                Address = addOrderRequest.Address,
                Comments = addOrderRequest.Comments,
                Phone = addOrderRequest.Phone,
                RegistrationDate = registrationDate,
            };

            var orderStatusHistory = new OrderStatusHistory
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                StatusId = 1,
                Date = registrationDate,
                Comment = "Поступление заказа"
            };

            await _dao.Orders.AddAsync(order);
            await _dao.OrderStatusHistories.AddAsync(orderStatusHistory);
            await _dao.SaveChangesAsync();
        }

        public async Task ChangeOrderStatusAsync(ChangeOrderStatusRequest changeOrderStatusRequest)
        {
            var order = await _dao.Orders.FindAsync(changeOrderStatusRequest.OrderId);
            if (order is null)
                throw new Exception("Заказа не существует");

            if (!_dao.OrderStatuses.Any(s => s.Id == changeOrderStatusRequest.StatusId))
                throw new Exception("Неверный ID статуса");

            var orderStatusHistory = new OrderStatusHistory
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                StatusId = changeOrderStatusRequest.StatusId,
                Date = DateTime.Now,
                Comment = "Поступление заказа"
            };

            order.StatusId = changeOrderStatusRequest.StatusId;
            await _dao.OrderStatusHistories.AddAsync(orderStatusHistory);
            await _dao.SaveChangesAsync();
        }

        public async Task<OrderShortResponse[]> GetAllOrdersAsync(int page, int ordersPerPage)
        {
            if(page < 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            if(ordersPerPage < 0)
                throw new ArgumentOutOfRangeException(nameof(ordersPerPage));

            return await _dao.Orders
            .Skip((page - 1) * ordersPerPage)
            .Take(ordersPerPage)
            .Select(o =>
            new OrderShortResponse
            {
                Id = o.Id,
                ClientName = o.ClientName,
                RegistratinDate = o.RegistrationDate,
                Status = o.Status.Name
            })
            .ToArrayAsync();
        }

        public async Task<OrderDetailResponse> GetOrderByIdAsync(Guid id)
        {
            var order = await _dao.Orders.FindAsync(id);
            if (order is null)
                throw new Exception("Заказа не существует");

            return new OrderDetailResponse
            {
                Id = order.Id,
                Address = order.Address,
                ClientName = order.ClientName,
                Comments = order.Comments,
                DeliveryTypeId = order.DeliveryTypeId,
                Phone = order.Phone,
                RegistratinDate = order.RegistrationDate,
                StatusId = order.StatusId
            };
        }
    }
}
