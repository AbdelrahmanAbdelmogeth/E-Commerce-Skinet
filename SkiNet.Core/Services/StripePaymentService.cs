using ECommerceSkinet.Core.Entities;
using ECommerceSkinet.Core.Entities.OrderAggregate;
using ECommerceSkinet.Core.Interfaces;
using ECommerceSkinet.Core.Specifications;
using EnumsCommerceSkinet.Core.Entities.OrderAggregate;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSkinet.Core.Services
{
    public class StripePaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _stripeConfiguration;
        public StripePaymentService(IBasketRepository basketRepository,
            IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _stripeConfiguration = configuration;
        }

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            // Set the Stripe API key from the configuration
            StripeConfiguration.ApiKey = _stripeConfiguration["StripeSettings:SecretKey"];

            // Retrieve the customer's basket using the basket repository
            var basket = await _basketRepository.GetBasketAsync(basketId);

            // If the basket is null, return null
            if (basket == null) return null;

            var shippingPrice = 0m;

            // If the basket has a delivery method, retrieve the delivery method and set the shipping price
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>()
                    .GetByIdAsync((int)basket.DeliveryMethodId);
                shippingPrice = deliveryMethod.Price;
            }

            // Update the price of each item in the basket to match the current price in the database
            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Entities.Product>().GetByIdAsync(item.Id);
                if (item.Price != productItem.Price)
                {
                    item.Price = productItem.Price;
                }
            }

            // Create a new instance of the PaymentIntentService
            var service = new PaymentIntentService();
            PaymentIntent intent;

            // If the basket does not have a payment intent ID, create a new payment intent
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                // If the basket already has a payment intent ID, update the existing payment intent
                /*
                    1.	Basket Modifications: Customers may add or remove items from their basket, or change the quantity of items. These changes affect the total amount that needs to be charged.
                    2.	Price Changes: The prices of items in the basket might change due to discounts, promotions, or price updates. The payment intent needs to reflect the current prices.
                    3.	Shipping Method Changes: Customers might change their selected shipping method, which can alter the shipping cost and thus the total amount.
                    4.	Avoiding Multiple Payment Intents: Creating a new payment intent every time there is a change would result in multiple payment intents for the same basket, which is inefficient and can lead to confusion.
                 */
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            // Update the basket in the repository with the new or updated payment intent details
            await _basketRepository.UpdateBasketAsync(basket);

            // Return the updated basket
            return basket;
        }


        public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentIdWithItemsSpecification(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
            if(order == null) return null;
            order.Status = OrderStatus.PaymentFailed;
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.Complete();
            return order;
        }

        public async Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentIdWithItemsSpecification(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
            if (order == null) return null;
            order.Status = OrderStatus.PaymentReceived;
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.Complete();
            return order;
        }

       
    }
}
