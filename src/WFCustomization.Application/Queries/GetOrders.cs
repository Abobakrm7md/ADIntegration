using System.Collections.Generic;
using WFCustomization.Application.DTOs;
using MediatR;

namespace WFCustomization.Application.Queries
{
    public class GetOrders : IRequest<IEnumerable<OrderDto>> { }
}
