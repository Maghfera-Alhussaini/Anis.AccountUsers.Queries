using Anis.AccountUsers.Queries.Domain.Events;
using Anis.AccountUsers.Queries.Grpc.Protos.History;
using Newtonsoft.Json;

namespace Anis.AccountUsers.Queries.Grpc.Extensions
{
    public static class RebuildExtension
    {
        public static Event<T> ToEvent<T>(this EventMessage body)
            => new()
            {
                AggregateId = Guid.Parse(body.AggregateId),
                Sequence = body.Sequence,
                UserId = body.UserId,
                Type = body.Type,
                Data = JsonConvert.DeserializeObject<T>(body.Data) ?? throw new ArgumentNullException("Data is not null here"),
                DateTime = body.DateTime.ToDateTime(),
                Version = body.Version
            };

    }
}
