using Anis.AccountUsers.Queries.Application.Features.Queries.GetAccountUsers;
using Anis.AccountUsers.Queries.Domain.Entities;

namespace Anis.AccountUsers.Queries.Appliaction.Contracts.Repositories
{
    public interface IAccountRepository : IAsyncRepository<Account>
    {
        Task<GetAccountUsersDto?> GetAccountUsersAsync(GetAccountUsersQuery query);
    }
}
