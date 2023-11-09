using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WFCustomization.Application.DTOs;
using WFCustomization.Application.Queries;
using WFCustomization.Core.Repositories;
using System.Threading;
using MediatR;
using WFCustomization.Infrastructure.Mappings;

namespace WFCustomization.Infrastructure.Queries.Handlers
{
    public class GetOrdersHandler : IRequestHandler<GetOrders, IEnumerable<OrderDto>>
    {
        private readonly IOrdersRepository _repository;

        public GetOrdersHandler(IOrdersRepository repository)
            => _repository = repository;

        public async Task<IEnumerable<OrderDto>> Handle(GetOrders query, CancellationToken cancellationToken)
            => (await _repository.BrowseAsync())
                ?.Select(order => order.AsDto());
    }
}
