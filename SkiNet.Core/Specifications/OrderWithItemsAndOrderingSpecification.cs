using ECommerceSkinet.Core.Entities.OrderAggregate;
using EnumsCommerceSkinet.Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSkinet.Core.Specifications
{
    public class OrderWithItemsAndOrderingSpecification : BaseSpecification<Order>
    {
        public OrderWithItemsAndOrderingSpecification(string email) : base(o => o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            AddOrderByDescending(o => o.OrderDate);
        }

        public OrderWithItemsAndOrderingSpecification(int id, string email) : base(o => o.Id == id && o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
        }

        public OrderWithItemsAndOrderingSpecification(OrderSpecParams specParams)
            :base(o => (string.IsNullOrEmpty(specParams.Status) || o.Status == ParseStatus(specParams.Status)))
        { 
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
            AddOrderByDescending(o => o.OrderDate);
        }

        public OrderWithItemsAndOrderingSpecification(int id)
            : base(o => o.Id == id)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
        }

        private static OrderStatus? ParseStatus(string status)
        {
            return Enum.TryParse<OrderStatus>(status, true, out var orderStatus) ? orderStatus : null;
        }
    }
}
