using Anis.AccountUsers.Queries.Appliaction.Contracts.Repositories;
using Anis.AccountUsers.Queries.Domain.Events;
using Anis.AccountUsers.Queries.Domain.Events.DataTypes;
using MediatR;

namespace Anis.AccountUsers.Queries.Appliaction.Features.Events
{
    public class UserDeletedFromAccountHandler : IRequestHandler<Event<UserDeletedFromAccountData>, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserDeletedFromAccountHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(Event<UserDeletedFromAccountData> @event, CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.Accounts.FindAsync(@event.AggregateId, true);

            if (account == null)
                return false;

            if (account.Sequence == @event.Sequence - 1)
            {
                account.IncreamentSequence();

                var user = await _unitOfWork.Users.FindAsync(@event.Data.UserId);

                if (user != null)
                    await _unitOfWork.Users.RemoveAsync(user);

                await _unitOfWork.SaveChangesAsync();
            }

            return account.Sequence >= @event.Sequence;
        }
    }
}
