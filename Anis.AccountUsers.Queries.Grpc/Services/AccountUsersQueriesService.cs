using Anis.AccountUsers.Queries.Grpc.Extensions;
using Anis.AccountUsers.Queries.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Anis.AccountUsers.Queries.Grpc.Services
{
    public class AccountUsersQueriesService : AccountUsersQueries.AccountUsersQueriesBase
    {
        private readonly IMediator _mediator;

        public AccountUsersQueriesService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override async Task<GetAccountUsersResponse> GetAccountUsers(GetAccountUsersRequest request, ServerCallContext context)
        {
            var query = request.ToQuery();

            var response = await _mediator.Send(query);

            return response.ToResponse();
        }
    }
}
