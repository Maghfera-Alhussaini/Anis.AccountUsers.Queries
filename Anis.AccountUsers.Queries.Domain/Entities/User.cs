using System.Security.Principal;

namespace Anis.AccountUsers.Queries.Domain.Entities
{
    public class User
    {
        public User(Guid id, Guid accountId)
        {
            Id = id;
            AccountId = accountId;
        }

        public Guid Id { get; private set; }
        public Guid AccountId { get; private set; }
        public Account? Account { get; private set; }

    }
}
