using System;

namespace WFCustomization.Application.Exceptions
{
    public class OrderNotFoundException : ApplicationException
    {
        public Guid OrderId { get; }

        public OrderNotFoundException(Guid orderId)
            : base($"Order with ID: {orderId} not found.")
                => OrderId = orderId;
    }
}
