using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WFCustomization.Core.Aggregates;

namespace WFCustomization.Core.Repositories
{
    public interface IOrdersRepository
    {
        Task<Order> GetAsync(Guid id);
        Task<IEnumerable<Order>> BrowseAsync();
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
    }
}
