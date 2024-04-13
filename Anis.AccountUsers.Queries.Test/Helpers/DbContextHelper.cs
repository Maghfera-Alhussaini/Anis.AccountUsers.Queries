using Anis.AccountUsers.Queries.Infra.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Anis.AccountUsers.Queries.Test.Helpers;

public class DbContextHelper
{
    private readonly IServiceProvider _provider;

    public DbContextHelper(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task<TResult> Query<TResult>(Func<AppDbContext, Task<TResult>> query)
    {
        using var scope = _provider.CreateScope();
        var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await query(appDbContext);
    }

    public async Task<T> InsertAsync<T>(T entity) where T : class
    {
        using var scope = _provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
        return entity;
    }
    public async Task<List<T>> InsertAsync<T>(List<T> entities) where T : class
    {
        using var scope = _provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Set<T>().AddRangeAsync(entities);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
        return entities;
    }
}