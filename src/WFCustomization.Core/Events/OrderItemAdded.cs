using WFCustomization.Shared.Kernel.BuildingBlocks;
using WFCustomization.Core.Entities;

namespace WFCustomization.Core.Events
{
    public class OrderItemAdded : IDomainEvent
    {
        public OrderItem OrderItem { get; }

        public OrderItemAdded(OrderItem orderItem)
            => OrderItem = orderItem;
    }
}
