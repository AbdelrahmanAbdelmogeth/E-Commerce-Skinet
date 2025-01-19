using ECommerceSkinet.Core.Entities;
using ECommerceSkinet.Core.Entities.OrderAggregate;
using ECommerceSkinet.Core.Interfaces;
using ECommerceSkinet.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSkinet.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;

        public OrderService(IUnitOfWork unitOfWork,
            IBasketRepository basketRepository
            )
        {
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
        }


        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, string paymentItemId, Address shippingAddress)
        {
            // get the basket from the repo
            var basket = await _basketRepository.GetBasketAsync(basketId);

            // get the items from product repo
            var items = new List<OrderItem>();
            foreach (var item in basket.Items) // Fix: Access the Items property of the basket
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if (productItem == null || productItem.Name == null || productItem.PictureUrl == null)
                {
                    throw new Exception("Product item or its properties cannot be null");
                }
                var orderItem = new OrderItem
                {
                    ItemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl),
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };
                items.Add(orderItem);
            }
            // get delivery method from repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // calc subtotal
            var subtotal = items.Sum(item => item.Price * item.Quantity);
            
            //create order
            var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal);
            _unitOfWork.Repository<Order>().Add(order);

            // save to db
            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            // delete basket
            await _basketRepository.DeleteBasketAsync(basketId);

            // return order
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrderWithItemsAndOrderingSpecification(id, buyerEmail);
            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderWithItemsAndOrderingSpecification(buyerEmail);
            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}
