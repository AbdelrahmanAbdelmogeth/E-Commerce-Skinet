using ECommerceSkinet.Core.Entities.OrderAggregate;
using Address = ECommerceSkinet.Core.Entities.OrderAggregate.Address;

namespace ECommerceSkinet.Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethod, string paymentMethodId, string basketId,
            Address shippingAddress);
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
        Task<Order> GetOrderByIdAsync(int id, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
