using Anis.AccountUsers.Queries.Infra.Persistence;
using Anis.AccountUsers.Queries.Infra.Services.ServiceBus;
using Anis.AccountUsers.Queries.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly: CollectionBehavior(DisableTestParallelization = TestConfig.UseSqlDatabase)]
namespace Anis.AccountUsers.Queries.Test.Helpers;
public static class ServiceCollectionExtensions
{
    public static void SetUnitTestsDefaultEnvironment(this IServiceCollection services)
    {
#pragma warning disable CS0162 // Unreachable code detected
        if (TestConfig.UseSqlDatabase)
            UseSqlDatabaseTesting(services);
        else
            UseInMemoryTesting(services);
#pragma warning restore CS0162 // Unreachable code detected

        RemoveServiceBusLogic(services);

    }

    private static void UseInMemoryTesting(IServiceCollection services)
    {
        var descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

        services.Remove(descriptor);

        var dbName = Guid.NewGuid().ToString();

        services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(dbName));
    }


    private static void UseSqlDatabaseTesting(IServiceCollection services)
    {
        var descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

        services.Remove(descriptor);

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer("Server=DESKTOP-GUDCQH7\\SQLEXPRESS; Database=Anis.AccountUsers.Query.Test; Integrated Security=true;TrustServerCertificate=True;");
        });

        services.AddHostedService<DbTruncate>();
    }

    private static void RemoveServiceBusLogic(IServiceCollection services)
    {
        var descriptor = services.Single(d => d.ImplementationType == typeof(AccountUsersListener));
        services.Remove(descriptor);
    }
}

public class DbTruncate : IHostedService
{
    private readonly IServiceProvider _provider;

    public DbTruncate(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var scope = _provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.MigrateAsync();

        context.RemoveRange(context.Users);
        context.RemoveRange(context.Accounts);

        await context.SaveChangesAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}