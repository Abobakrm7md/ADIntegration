using System;
using System.Collections.Generic;
using WFCustomization.Application.Commands.WriteModels;
using MediatR;

namespace WFCustomization.Application.Commands
{
    public record UpdateOrder(Guid Id, Guid BuyerId, AddressWriteModel ShippingAddress, IEnumerable<OrderItemWriteModel> Items, string Status) : IRequest;
}
