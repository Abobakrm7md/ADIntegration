using WFCustomization.Core.Repositories;
using System.Threading.Tasks;
using WFCustomization.Core.Aggregates;
using WFCustomization.Core.Types;
using WFCustomization.Application.Exceptions;
using Microsoft.Extensions.Logging;
using MediatR;
using System.Threading;

namespace WFCustomization.Application.Commands.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrder>
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly ILogger<CreateOrderHandler> _logger;

        public CreateOrderHandler(IOrdersRepository ordersRepository, ILogger<CreateOrderHandler> logger)
        {
            _ordersRepository = ordersRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateOrder command, CancellationToken cancellationToken)
        {
            var order = await _ordersRepository.GetAsync(command.Id);

            if(order is not null)
            {
                throw new OrderAlreadyExistsException(command.Id);
            }

            order = new Order(command.Id, command.BuyerId, command.ShippingAddress.AsValueObject(), command.Items.AsEntities(), OrderStatus.Pending);

            await _ordersRepository.AddAsync(order);

            _logger.LogInformation($"Order with ID: {order.Id} has been created.");

            return Unit.Value;
        }
    }
}
