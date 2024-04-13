using Anis.AccountUsers.Queries.Domain.Events;
using Anis.AccountUsers.Queries.Domain.Events.DataTypes;
using Anis.AccountUsers.Queries.Infra.Constants;

namespace Anis.AccountUsers.Queries.Test.Fakers
{
    public class UserDeletedFromAccountFaker : CustomConstructorFaker<Event<UserDeletedFromAccountData>>
    {
        public UserDeletedFromAccountFaker()
        {
            RuleFor(r => r.AggregateId, f => f.Random.Guid());
            RuleFor(r => r.Sequence, 1);
            RuleFor(r => r.UserId, f => f.Random.Guid().ToString());
            RuleFor(r => r.Version, 1);
            RuleFor(r => r.DateTime, DateTime.UtcNow);
            RuleFor(r => r.Type, EventType.UserAssignedToAccount);
            RuleFor(r => r.Data, new UserDeletedFromAccountDataFaker().Generate());
        }
        public UserDeletedFromAccountFaker(Guid aggregateId, int sequence)
        {
            RuleFor(r => r.AggregateId, aggregateId);
            RuleFor(r => r.Sequence, sequence);
            RuleFor(r => r.UserId, f => f.Random.Guid().ToString());
            RuleFor(r => r.Version, 1);
            RuleFor(r => r.DateTime, DateTime.UtcNow);
            RuleFor(r => r.Type, EventType.UserDeletedFromAccount);
            RuleFor(r => r.Data, new UserDeletedFromAccountDataFaker().Generate());
        }
        public UserDeletedFromAccountFaker(Guid aggregateId, Guid userId, int sequence)
        {
            RuleFor(r => r.AggregateId, aggregateId);
            RuleFor(r => r.Sequence, sequence);
            RuleFor(r => r.UserId, f => f.Random.Guid().ToString());
            RuleFor(r => r.Version, 1);
            RuleFor(r => r.DateTime, DateTime.UtcNow);
            RuleFor(r => r.Type, EventType.UserDeletedFromAccount);
            RuleFor(r => r.Data, new UserDeletedFromAccountDataFaker(userId));
        }
    }

    public class UserDeletedFromAccountDataFaker : CustomConstructorFaker<UserDeletedFromAccountData>
    {
        public UserDeletedFromAccountDataFaker()
        {
            RuleFor(r => r.UserId, f => f.Random.Guid());
        }
        public UserDeletedFromAccountDataFaker(Guid userId)
        {
            RuleFor(r => r.UserId, userId);
        }
    }
}
