using System.Linq;
using WFCustomization.Application.DTOs;
using WFCustomization.Core.Aggregates;

namespace WFCustomization.Infrastructure.Mappings
{
    public static class Extensions
    {
        public static OrderDto AsDto(this Order order)
            => new OrderDto
            {
                Id = order.Id,
                BuyerId = order.BuyerId,
                ShippingAddress = new AddressDto
                {
                    City = order.ShippingAddress.City,
                    Street = order.ShippingAddress.Street,
                    Province = order.ShippingAddress.Province,
                    Country = order.ShippingAddress.Country,
                    ZipCode = order.ShippingAddress.ZipCode
                },
                Status = order.Status.ToString().ToLowerInvariant(),
                TotalPrice = order.TotalPrice,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(item => new OrderItemDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Price = item.Price
                })
            };
    }
}
