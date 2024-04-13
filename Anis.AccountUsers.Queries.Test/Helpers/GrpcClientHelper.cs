using Anis.AccountUsers.Queries.Grpc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Anis.AccountUsers.Queries.Test.Helpers;

public class GrpcClientHelper
{
    private readonly WebApplicationFactory<Program> _factory;

    public GrpcClientHelper(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    public TResult Query<TResult>(Func<Protos.AccountUsersQueries.AccountUsersQueriesClient, TResult> send)
    {
        var client = new Protos.AccountUsersQueries.AccountUsersQueriesClient(_factory.CreateGrpcChannel());
        return send(client);
    }
}