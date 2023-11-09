using System.Threading;
using System.Threading.Tasks;
using WFCustomization.Application.DTOs;
using WFCustomization.Application.Queries;
using WFCustomization.Core.Repositories;
using WFCustomization.Infrastructure.Mappings;
using MediatR;

namespace WFCustomization.Infrastructure.Queries.Handlers
{
    public class GetOrderHandler : IRequestHandler<GetOrder, OrderDto>
    {
        private readonly IOrdersRepository _repository;

        public GetOrderHandler(IOrdersRepository repository)
            => _repository = repository;

        public async Task<OrderDto> Handle(GetOrder query, CancellationToken cancellationToken)
            => (await _repository.GetAsync(query.Id))
                ?.AsDto();
    }
}
