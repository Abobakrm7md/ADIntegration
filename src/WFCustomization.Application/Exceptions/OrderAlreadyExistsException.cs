using System;
using ApplicationException = WFCustomization.Shared.Exceptions.ApplicationException;

namespace WFCustomization.Application.Exceptions
{
    public class OrderAlreadyExistsException : ApplicationException
    {
        public Guid OrderId { get; }

        public OrderAlreadyExistsException(Guid orderId)
            : base($"Order with Id: {orderId} already exists.")
                => OrderId = orderId;
    }
}
