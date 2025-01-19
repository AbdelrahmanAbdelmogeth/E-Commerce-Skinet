using ECommerceSkinet.Core.Entities.OrderAggregate;
using EnumsCommerceSkinet.Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECommerceSkinet.Infrastructure.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder) {
            builder.OwnsOne(o => o.ShipToAddress, a =>
            {
                a.WithOwner();
            });

            builder.Property(s => s.Status)
                .HasConversion(
                o => o.ToString(),
                o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o.ToString())
                );

            //when we delete an order we also delete the order items as well
            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
