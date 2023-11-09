using System;
using System.Threading;
using System.Threading.Tasks;
using WFCustomization.Application.Exceptions;
using WFCustomization.Core.Exceptions;
using WFCustomization.Core.Repositories;
using WFCustomization.Core.Types;
using MediatR;
using Microsoft.Extensions.Logging;

namespace WFCustomization.Application.Commands.Handlers
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrder>
    {
        private readonly IOrdersRepository _repository;
        private readonly ILogger<UpdateOrderHandler> _logger;

        public UpdateOrderHandler(IOrdersRepository repository, ILogger<UpdateOrderHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateOrder command, CancellationToken cancellationToken)
        {
            if(!Enum.TryParse<OrderStatus>(command.Status, true, out var status))
            {
                throw new InvalidOrderStatusException(command.Status);
            }

            var order = await _repository.GetAsync(command.Id);

            if(order is null)
            {
                throw new OrderNotFoundException(command.Id);
            }

            order.Update(command.BuyerId, command.ShippingAddress.AsValueObject(), command.Items.AsEntities(), status);

            await _repository.UpdateAsync(order);

            _logger.LogInformation($"Order with ID: '{command.Id}' has been updated.");

            return Unit.Value;
        }
    }
}
