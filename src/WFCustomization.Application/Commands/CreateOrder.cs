using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WFCustomization.Application.Commands.WriteModels;
using WFCustomization.Core;
using MediatR;

namespace WFCustomization.Application.Commands
{
    public record CreateOrder([Required] Guid BuyerId, [Required] AddressWriteModel ShippingAddress, [Required] IEnumerable<OrderItemWriteModel> Items) : IRequest
    {
        public Guid Id { get; init; } = new OrderId(Guid.NewGuid());
    }
}
