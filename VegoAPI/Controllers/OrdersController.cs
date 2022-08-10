using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VegoAPI.Models.RequestModels;
using VegoAPI.Services.OrdersRepository;
using VegoAPI.Utils;

namespace VegoAPI.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrdersController(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public async Task<IActionResult> AddOrder(AddOrderRequest addOrderRequest)
        {
            try
            {
                await _ordersRepository.AddOrderAsync(addOrderRequest);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }
        }
    }
}
