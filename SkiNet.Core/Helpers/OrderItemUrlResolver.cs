using AutoMapper;
using ECommerceSkinet.Core.DTO;
using ECommerceSkinet.Core.Entities.OrderAggregate;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSkinet.Core.Helpers
{
    public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration config;

        public OrderItemUrlResolver(IConfiguration config)
        {
            this.config = config;
        }

        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ItemOrdered.PictureUrl))
            {
                return config["ApiUrl"] + source.ItemOrdered.PictureUrl;
            }
            return null;
        }
    }
}
