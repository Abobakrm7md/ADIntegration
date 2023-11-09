using WFCustomization.Core.Aggregates;
using WFCustomization.Shared.Kernel.BuildingBlocks;

namespace WFCustomization.Core.Events
{
    public class OrderUpdated : IDomainEvent
    {
        public Order Order { get; }

        public OrderUpdated(Order order)
            => Order = order;
    }
}
