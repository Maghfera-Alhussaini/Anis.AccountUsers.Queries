using Anis.AccountUsers.Queries.Application.Features.Queries.GetAccountUsers;
using Anis.AccountUsers.Queries.Grpc.Protos;

namespace Anis.AccountUsers.Queries.Grpc.Extensions
{
    public static class ResponseExtensions
    {
        public static GetAccountUsersResponse ToResponse(this GetAccountUsersDto dto)
            => new()
            {
                AccountId = dto.AccountId.ToString(),
                Users =
                {
                    dto.Users.Select(s=>s.ToString())
                }
            };
    }
}
