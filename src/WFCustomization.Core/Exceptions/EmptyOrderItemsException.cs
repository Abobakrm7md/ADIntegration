using System;
using WFCustomization.Shared.Exceptions;

namespace WFCustomization.Core.Exceptions
{
    public class EmptyOrderItemsException : DomainException
    {
        public Guid OrderId { get; }

        public EmptyOrderItemsException(Guid orderId)
            : base($"Empty order items defined for order with ID: {orderId}")
                => OrderId = orderId;
    }
}
