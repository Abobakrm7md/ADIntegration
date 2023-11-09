using System;
using WFCustomization.Application.DTOs;
using MediatR;

namespace WFCustomization.Application.Queries
{
    public class GetOrder : IRequest<OrderDto>
    {
        public Guid Id { get; set; }
    }
}
