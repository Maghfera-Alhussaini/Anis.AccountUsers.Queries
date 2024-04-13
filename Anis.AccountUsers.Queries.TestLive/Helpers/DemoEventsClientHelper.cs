using Anis.AccountUsers.Queries.TestLive.Protos;
using Bogus.DataSets;
using Grpc.Net.Client;

namespace Anis.AccountUsers.Queries.TestLive.Helpers
{
    public class DemoEventsClientHelper
    {
        public static TResult Query<TResult>(Func<AccountUsersDemoEvents.AccountUsersDemoEventsClient , TResult> send)
        {
            var channel = GrpcChannel.ForAddress(Address.Base);
            var client = new AccountUsersDemoEvents.AccountUsersDemoEventsClient(channel);
            return send(client);
        }
    }
}