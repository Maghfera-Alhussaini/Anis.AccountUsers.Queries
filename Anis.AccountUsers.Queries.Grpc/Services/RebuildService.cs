using Anis.AccountUsers.Queries.Domain.Events.DataTypes;
using Anis.AccountUsers.Queries.Grpc.Extensions;
using Anis.AccountUsers.Queries.Grpc.Protos;
using Anis.AccountUsers.Queries.Grpc.Protos.History;
using Anis.AccountUsers.Queries.Infra.Constants;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;

namespace Anis.AccountUsers.Queries.Grpc.Services
{
    public class RebuildService : AccountUsersRebuilder.AccountUsersRebuilderBase
    {
        private readonly AccountUsersEventHistory.AccountUsersEventHistoryClient _eventHistoryClient;
        private readonly IMediator _mediator;
        private readonly ILogger<RebuildService> _logger;

        public RebuildService(AccountUsersEventHistory.AccountUsersEventHistoryClient eventHistoryClient, IMediator mediator, ILogger<RebuildService> logger)
        {
            _eventHistoryClient = eventHistoryClient;
            _mediator = mediator;
            _logger = logger;
        }

        public async override Task<Empty> BuildAccountUsers(BuildRequest request, ServerCallContext context)
        {
            for (int i = request.Page; i > 0; i++)
            {
                var getEvent = new GetEventsRequest()
                {
                    CurrentPage = i,
                    PageSize = request.Size,
                };

                _logger.LogWarning("Start Handling page {page}", i);

                var response = await _eventHistoryClient.GetEventsAsync(getEvent);

                if (response.Events.Count > 0)
                {
                    await HandleResponseAsync(response);

                    _logger.LogWarning("End Handling page {page}", i);

                }
                else
                {
                    _logger.LogWarning("End page {page}", i);
                    break;
                }
            }

            return new Empty();
        }

        private async Task HandleResponseAsync(Response response)
        {
            foreach (var @event in response.Events)
            {
                switch (@event.Type)
                {
                    case EventType.UserAssignedToAccount:
                        await _mediator.Send(@event.ToEvent<UserAssignedToAccountData>());
                        break;

                    case EventType.UserDeletedFromAccount:
                        await _mediator.Send(@event.ToEvent<UserDeletedFromAccountData>());
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(@event.Type, "Event Type out of range in rebuild service");
                }
            }
        }
    }
}
