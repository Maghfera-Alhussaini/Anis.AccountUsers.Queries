using System.ComponentModel.DataAnnotations;

namespace Anis.AccountUsers.Queries.Grpc.Setup
{
    public class ExternalServiceOptions
    {
        public const string ExternalServices = "ExternalServices";

        [Required]
        [DataType(DataType.Url)]
        public required string AccountUsersCommandUrl { get; init; }

    }
}
