using Anis.AccountUsers.Queries.Domain.Events;
using Anis.AccountUsers.Queries.Domain.Events.DataTypes;
using Anis.AccountUsers.Queries.Infra.Constants;

namespace Anis.AccountUsers.Queries.Test.Fakers
{
    public class UserAssignedToAccountFaker : CustomConstructorFaker<Event<UserAssignedToAccountData>>
    {
        public UserAssignedToAccountFaker()
        {
            RuleFor(r => r.AggregateId, f => f.Random.Guid());
            RuleFor(r => r.Sequence, 1);
            RuleFor(r => r.UserId, f => f.Random.Guid().ToString());
            RuleFor(r => r.Version, 1);
            RuleFor(r => r.DateTime, DateTime.UtcNow);
            RuleFor(r => r.Type, EventType.UserAssignedToAccount);
            RuleFor(r => r.Data, new UserAssignedToAccountDataFaker().Generate());
        }

        public UserAssignedToAccountFaker(Guid aggregateId, int sequence, Guid? userId = null)
        {
            RuleFor(r => r.AggregateId, f => aggregateId);
            RuleFor(r => r.Sequence, sequence);
            RuleFor(r => r.UserId, f => f.Random.Guid().ToString());
            RuleFor(r => r.Version, 1);
            RuleFor(r => r.DateTime, DateTime.UtcNow);
            RuleFor(r => r.Type, EventType.UserAssignedToAccount);
            RuleFor(r => r.Data, new UserAssignedToAccountDataFaker(userId).Generate());
        }
    }

    public class UserAssignedToAccountDataFaker : CustomConstructorFaker<UserAssignedToAccountData>
    {
        public UserAssignedToAccountDataFaker()
        {
            RuleFor(r => r.UserId, f => f.Random.Guid());
        }
        public UserAssignedToAccountDataFaker(Guid? userId)
        {
            RuleFor(r => r.UserId, f => userId ?? f.Random.Guid());
        }
    }
}
