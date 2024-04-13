

using Anis.AccountUsers.Queries.Domain.Entities;

namespace Anis.AccountUsers.Queries.Appliaction.Contracts.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAsyncRepository<User> Users { get; }

        IAccountRepository Accounts { get; }
        Task SaveChangesAsync();
    }
}
