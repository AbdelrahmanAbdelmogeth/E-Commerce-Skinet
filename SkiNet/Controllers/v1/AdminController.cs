using AutoMapper;
using ECommerceSkinet.Core.DTO;
using ECommerceSkinet.Core.Entities;
using ECommerceSkinet.Core.Entities.OrderAggregate;
using ECommerceSkinet.Core.Helpers;
using ECommerceSkinet.Core.Interfaces;
using ECommerceSkinet.Core.Services;
using ECommerceSkinet.Core.Specifications;
using ECommerceSkinet.WebAPI.Errors;
using EnumsCommerceSkinet.Core.Entities.OrderAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSkinet.WebAPI.Controllers.v1
{
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    [ApiVersion("1.0")]
    public class AdminController(IUnitOfWork unitOfWork, IMapper mapper, StripePaymentService paymentService) : CustomControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private IPaymentService _paymentService = paymentService;


        [HttpGet("orders")]
        public async Task<ActionResult<IReadOnlyList<OrderAdminToReturnDto>>> GetOrders([FromQuery] OrderSpecParams specParams)
        {
            var spec = new OrderWithItemsAndOrderingSpecification(specParams);
            var totalItems = await unitOfWork.Repository<Core.Entities.OrderAggregate.Order>().CountAsync(spec);
            var orders = await unitOfWork.Repository<Core.Entities.OrderAggregate.Order>().ListAsync(spec);
            // convert status from 0,1,2 to text
            if (orders != null && orders.Count != 0)
            {
                // Map orders to DTOs
                var orderDtos = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderAdminToReturnDto>>(orders);

                // Return paginated response using the new overload 
                return CreatePagedResult(orderDtos, specParams.PageIndex, specParams.PageSize, totalItems);
            }

            return new ObjectResult(new ApiResponse(404));
        }

        [HttpGet("orders/{id:int}")]
        public async Task<ActionResult<OrderAdminToReturnDto>> GetOrder(int id)
        {
            var spec = new OrderWithItemsAndOrderingSpecification(id);
            var order = await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
            if (order == null) return BadRequest("No order with that id");
            return Ok(_mapper.Map<OrderAdminToReturnDto>(order));
        }

        [HttpPost("orders/refund/{id:int}")]
        public async Task<ActionResult<OrderAdminToReturnDto>> RefundOrder(int id)
        {
            var spec = new OrderWithItemsAndOrderingSpecification(id);
            var order = await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
            if (order == null) return BadRequest("No order with that id");
            if (order.Status == OrderStatus.Pending) return BadRequest("Payment not recieved for that order");

            var result = await _paymentService.RefundPayment(order.PaymentIntentId);
            if (result == "succedded")
            {
                order.Status = OrderStatus.PaymentRefunded;
                await unitOfWork.Complete();
                return Ok(_mapper.Map<OrderAdminToReturnDto>(order)); ;
            };

            return BadRequest("Problem refunding order");
        }
    }
}