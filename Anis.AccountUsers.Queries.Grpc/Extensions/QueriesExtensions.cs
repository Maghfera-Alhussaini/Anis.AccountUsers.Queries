
using Anis.AccountUsers.Queries.Application.Features.Queries.GetAccountUsers;
using Anis.AccountUsers.Queries.Grpc.Protos;

namespace Anis.AccountUsers.Queries.Grpc.Extensions
{
    public static class QueriesExtensions
    {
        public static GetAccountUsersQuery ToQuery(this GetAccountUsersRequest request)
            => new(Guid.Parse(request.AccountId));
    }
}
