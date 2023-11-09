using System;
using WFCustomization.Shared.Kernel.BuildingBlocks;

namespace WFCustomization.Core
{
    public class BuyerId : TypedIdValueBase
    {
        public BuyerId(Guid value)
            : base(value) { }

        public static implicit operator BuyerId(Guid buyerId)
            => new BuyerId(buyerId);
    }
}
