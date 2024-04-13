using Anis.AccountUsers.Queries.Appliaction.Contracts.Repositories;
using Anis.AccountUsers.Queries.Domain.Entities;
using Anis.AccountUsers.Queries.Domain.Events;
using Anis.AccountUsers.Queries.Domain.Events.DataTypes;
using MediatR;

namespace Anis.AccountUsers.Queries.Appliaction.Features.Events
{
    public class UserAssignedToAccountHandler : IRequestHandler<Event<UserAssignedToAccountData>, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserAssignedToAccountHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(Event<UserAssignedToAccountData> @event, CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.Accounts.FindAsync(@event.AggregateId, true);

          
            if (account == null)
            {
                account = Account.Create(@event.AggregateId, @event.Sequence);

                var user = new User(@event.Data.UserId, @event.AggregateId);

                await _unitOfWork.Accounts.AddAsync(account);

                await _unitOfWork.Users.AddAsync(user);

                await _unitOfWork.SaveChangesAsync();

                return true;
            }

            //    4                   6      - 1 
            if (account.Sequence == @event.Sequence - 1)
            {
                account.IncreamentSequence(); //5 

                var user = new User(@event.Data.UserId, @event.AggregateId);

                await _unitOfWork.Users.AddAsync(user);

                await _unitOfWork.SaveChangesAsync();
            }

            //          5                 5
            return account.Sequence >= @event.Sequence;
        }
    }
}
