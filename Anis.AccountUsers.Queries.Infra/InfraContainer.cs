using Anis.AccountUsers.Queries.Appliaction.Contracts.Repositories;
using Anis.AccountUsers.Queries.Infra.Persistence;
using Anis.AccountUsers.Queries.Infra.Persistence.Repositories;
using Anis.AccountUsers.Queries.Infra.Services.ServiceBus;
using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Anis.AccountUsers.Queries.Infra
{
    public static class InfraContainer
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DataBase")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton(s =>
            {
                return new ServiceBusClient(configuration.GetConnectionString("ServiceBus"));
            });

            services.AddHostedService<AccountUsersListener>();

            return services;
        }

    }
}
