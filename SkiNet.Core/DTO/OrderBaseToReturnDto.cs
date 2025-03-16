using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceSkinet.Core.Entities.OrderAggregate;
using EnumsCommerceSkinet.Core.Entities.OrderAggregate;

namespace ECommerceSkinet.Core.DTO
{
    public class OrderBaseToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public Address ShipToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public string DeliveryTime { get; set; }
        public decimal ShippingPrice { get; set; }
        public IReadOnlyCollection<OrderItemDto> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
    }
}
