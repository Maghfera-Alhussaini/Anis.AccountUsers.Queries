using Anis.AccountUsers.Queries.Appliaction.Contracts.Repositories;
using Anis.AccountUsers.Queries.Application.Features.Queries.GetAccountUsers;
using Anis.AccountUsers.Queries.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Anis.AccountUsers.Queries.Infra.Persistence.Repositories
{
    public class AccountRepository : AsyncRepository<Account>, IAccountRepository
    {
        private readonly AppDbContext _appDbContext;

        public AccountRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async override Task<Account?> FindAsync(Guid id, bool includeRelated = false)
        {
            if (includeRelated)
                return await _appDbContext.Accounts.Include(a => a.Users)
                                                   .FirstOrDefaultAsync(a => a.Id == id);

            return await base.FindAsync(id, includeRelated);
        }
        public async Task<GetAccountUsersDto?> GetAccountUsersAsync(GetAccountUsersQuery query)
        {
            return await _appDbContext.Accounts.Where(a => a.Id == query.AccountId)
                            .Select(a => new GetAccountUsersDto
                            {
                                AccountId = a.Id,
                                Users = a.Users.Select(u => u.Id).ToList()
                            }).FirstOrDefaultAsync();

        }
    }
}
