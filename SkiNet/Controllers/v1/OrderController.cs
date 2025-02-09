using AutoMapper;
using ECommerceSkinet.Core.DTO;
using ECommerceSkinet.Core.Entities.OrderAggregate;
using ECommerceSkinet.Core.Interfaces;
using ECommerceSkinet.WebAPI.Controllers;
using ECommerceSkinet.WebAPI.Errors;
using ECommerceSkinet.WebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Controllers.v1
{
    [Authorize]
    [ApiVersion("1.0")]
    public class OrdersController : CustomControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            //var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new ApiResponse(404, "User email not found"));
            }
            var address = mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);
            var order = await orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, orderDto.PaymentIntentId, address);
            if (order == null)
            {
                return BadRequest(new ApiResponse(400, "Problem creating order"));
            }

            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal() ?? string.Empty;
            var orders = await orderService.GetOrdersForUserAsync(email);
            return Ok(mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal() ?? string.Empty;
            var order = await orderService.GetOrderByIdAsync(id, email);
            if (order == null)
                return NotFound(new ApiResponse(404));
            return Ok(mapper.Map<Order, OrderToReturnDto>(order));
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await orderService.GetDeliveryMethodsAsync());
        }
    }
}
