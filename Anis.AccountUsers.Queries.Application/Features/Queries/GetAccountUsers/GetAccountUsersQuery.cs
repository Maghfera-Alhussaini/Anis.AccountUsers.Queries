using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Queries.Application.Features.Queries.GetAccountUsers
{
    public record GetAccountUsersQuery(Guid AccountId) : IRequest<GetAccountUsersDto>;
}
