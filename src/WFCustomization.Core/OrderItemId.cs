using System;
using WFCustomization.Shared.Kernel.BuildingBlocks;

namespace WFCustomization.Core
{
    public sealed class OrderItemId : TypedIdValueBase
    {
        public OrderItemId(Guid value)
            : base(value) { }

        public static implicit operator OrderItemId(Guid orderItemId)
            => new OrderItemId(orderItemId);
    }
}
