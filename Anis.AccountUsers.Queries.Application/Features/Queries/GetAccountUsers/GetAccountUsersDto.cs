using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Queries.Application.Features.Queries.GetAccountUsers
{
    public class GetAccountUsersDto
    {
        public Guid AccountId { get; set; }
        public List<Guid> Users { get; set; } = new List<Guid>();
    }
}
