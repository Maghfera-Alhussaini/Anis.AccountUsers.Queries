using Anis.AccountUsers.Queries.Appliaction.Contracts.Repositories;
using Anis.AccountUsers.Queries.Domain.Entities;

namespace Anis.AccountUsers.Queries.Infra.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        private IAccountRepository? _accounts;

        public IAccountRepository Accounts
        {
            get
            {
                if (_accounts != null)
                    return _accounts;

                return _accounts = new AccountRepository(_appDbContext);
            }
        }

        private IAsyncRepository<User>? _users;

        public IAsyncRepository<User> Users
        {
            get
            {
                if (_users != null)
                    return _users;

                return _users = new AsyncRepository<User>(_appDbContext);
            }
        }


        public void Dispose()
        {
            _appDbContext.Dispose();
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
