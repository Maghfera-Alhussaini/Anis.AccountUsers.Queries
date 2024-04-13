using Anis.AccountUsers.Queries.Domain.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Anis.AccountUsers.Queries.Test.Helpers
{
    public class HandlerHelper
    {
        private readonly IServiceProvider _provider;

        public HandlerHelper(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<bool> HandleMessage<T>(Event<T> message)
        {
            using var scope = _provider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            return await mediator.Send(message);
        }
    }
}