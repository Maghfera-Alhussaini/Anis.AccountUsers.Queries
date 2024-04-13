using Anis.AccountUsers.Queries.Domain.Entities;

namespace Anis.AccountUsers.Queries.Test.Fakers
{
    public class AccountFaker : CustomConstructorFaker<Account>
    {
        public AccountFaker(Guid? id = null, int sequence = 1)
        {
            RuleFor(r => r.Id, r => id ?? r.Random.Guid());
            RuleFor(r => r.Sequence, sequence);
        }

        public static Account CreateWithSingleUser(Guid id, Guid? userId = null)
        {
            return new AccountFaker(id)
                        .RuleFor(r => r.Users, [new UserFaker(userId, id).Generate()]);
        }
    }

    public class UserFaker : CustomConstructorFaker<User>
    {
        public UserFaker(Guid? id, Guid accountId)
        {
            RuleFor(u => u.Id, r => id ?? r.Random.Guid());
            RuleFor(r => r.AccountId, accountId);
        }
    }
}
