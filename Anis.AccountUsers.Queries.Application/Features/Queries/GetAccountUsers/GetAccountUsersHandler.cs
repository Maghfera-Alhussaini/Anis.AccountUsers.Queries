using Anis.AccountUsers.Queries.Appliaction.Contracts.Repositories;
using Anis.AccountUsers.Queries.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Queries.Application.Features.Queries.GetAccountUsers
{
    public class GetAccountUsersHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAccountUsersQuery, GetAccountUsersDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<GetAccountUsersDto> Handle(GetAccountUsersQuery query, CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.Accounts.GetAccountUsersAsync(query);

            return account == null ? throw new AppException(ExceptionStatusCode.NotFound, "Account Not Found") : account;
        }
    }
}
