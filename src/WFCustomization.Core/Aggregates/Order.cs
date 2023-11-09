using System;
using System.Collections.Generic;
using System.Linq;
using WFCustomization.Shared.Kernel.BuildingBlocks;
using WFCustomization.Core.Entities;
using WFCustomization.Core.Events;
using WFCustomization.Core.Exceptions;
using WFCustomization.Core.Rules;
using WFCustomization.Core.Types;
using WFCustomization.Core.ValueObjects;

namespace WFCustomization.Core.Aggregates
{
    public class Order : AggregateRoot
    {
        private ISet<OrderItem> _items = new HashSet<OrderItem>();

        public OrderId Id { get; private set; }
        public BuyerId BuyerId { get; private set; }
        public Address ShippingAddress { get; private set; }
        public OrderStatus Status { get; private set; }
        public Amount TotalPrice { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public IEnumerable<OrderItem> Items
        {
            get { return _items; }
            private set { _items = new HashSet<OrderItem>(value); }
        }

        private Order() { }

        public Order(OrderId id, BuyerId buyerId, Address shippingAddress, IEnumerable<OrderItem> orderItems, OrderStatus status, int? version = null)
        {
            Id = id;
            BuyerId = buyerId;
            ShippingAddress = shippingAddress;
            Items = orderItems ?? throw new EmptyOrderItemsException(id);
            Status = status;
            TotalPrice = Items.Sum(item => item.Price);

            CheckRule(new MinimumAmountOfASingleOrderShouldBeAtLeast10(TotalPrice));
            CheckRule(new AmountOfASingleOrderCannotExceed100k(TotalPrice));

            Version = version ?? 1;
            CreatedAt = DateTime.UtcNow;
        }

        public void AddOrderItem(OrderItem newItem)
        {
            var item = _items.SingleOrDefault(x => x.Id.Equals(newItem.Id));

            if(!(item is null))
            {
                throw new OrderItemAlreadyExistsException(newItem.Id);
            }

            _items.Add(newItem);

            AddDomainEvent(new OrderItemAdded(newItem));
        }

        public void Update(Guid buyerId, Address shippingAddress, IEnumerable<OrderItem> items, OrderStatus status)
        {
            BuyerId = buyerId;
            ShippingAddress = shippingAddress;
            Items = items;
            Status = status;

            AddDomainEvent(new OrderUpdated(this));
        }
    }
}
