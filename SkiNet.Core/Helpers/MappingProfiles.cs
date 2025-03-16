using AutoMapper;
using ECommerceSkinet.Core.DTO;
using ECommerceSkinet.Core.Entities;
using ECommerceSkinet.Core.Entities.Identity;
using ECommerceSkinet.Core.Entities.OrderAggregate;

namespace ECommerceSkinet.Core.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
            CreateMap<Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryTime, o => o.MapFrom(s => s.DeliveryMethod.DeliveryTime))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));
            // New mappings for Order
            CreateMap<Order, OrderBaseToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryTime, o => o.MapFrom(s => s.DeliveryMethod.DeliveryTime))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString())); // Map status to string
            CreateMap<Order, OrderAdminToReturnDto>()
                .IncludeBase<Order, OrderBaseToReturnDto>() // Inherit mappings from OrderBaseDto
                .ForMember(d => d.PaymentIntentId, o => o.MapFrom(s => s.PaymentIntentId)); // Map admin-only property
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
        }
    }
}
