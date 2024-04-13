using Anis.AccountUsers.Queries.Grpc.Protos;
using FluentValidation;

namespace Anis.AccountUsers.Queries.Grpc.Validatiors
{
    public class GetAccountUsersRequestValidator: AbstractValidator<GetAccountUsersRequest>
    {
        public GetAccountUsersRequestValidator() { 
        RuleFor(r => r.AccountId)
           .Must(accountId => Guid.TryParse(accountId, out _))
                .NotEqual(Guid.Empty.ToString());
        }
    }
}
