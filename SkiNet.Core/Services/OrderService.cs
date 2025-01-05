using ECommerceSkinet.Core.Entities;
using ECommerceSkinet.Core.Entities.OrderAggregate;
using ECommerceSkinet.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSkinet.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepp;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IBasketRepository _basketRepository;
        private readonly IGenericRepository<DeliveryMethod> _dmRepo;

        public OrderService(IGenericRepository<Order> orderRepo,
            IGenericRepository<DeliveryMethod> dmRepo,
            IGenericRepository<Product> productRepo,
            IBasketRepository basketRepository
            )
        {

            _orderRepp = orderRepo;
            _dmRepo = dmRepo;
            _productRepo = productRepo;
            _basketRepository = basketRepository;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            // get the basket from the repo
            var basket = await _basketRepository.GetBasketAsync(basketId);

            // get the items from product repo
            var items = new List<OrderItem>();
            foreach (var item in basket.Items) // Fix: Access the Items property of the basket
            {
                var productItem = await _productRepo.GetByIdAsync(item.Id);
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
            var deliveryMethod = await _dmRepo.GetByIdAsync(deliveryMethodId);

            // calc subtotal
            var subtotal = items.Sum(item => item.Price * item.Quantity);

            // save to db
            var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal);
            //TODO: Save to db

            // return order
            return order;
        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
