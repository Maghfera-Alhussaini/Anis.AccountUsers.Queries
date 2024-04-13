using Anis.AccountUsers.Queries.Appliaction;
using Anis.AccountUsers.Queries.Grpc.ExceptionHandler;
using Anis.AccountUsers.Queries.Grpc.Interceptors;
using Anis.AccountUsers.Queries.Grpc.Services;
using Anis.AccountUsers.Queries.Grpc.Setup;
using Anis.AccountUsers.Queries.Grpc.Validatiors.Main;
using Anis.AccountUsers.Queries.Infra;
using Anis.AccountUsers.Queries.Infra.Persistence;
using Anis.AccountUsers.Queries.Infra.Services.Logger;
using Calzolari.Grpc.AspNetCore.Validation;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Anis.AccountUsers.Queries.Grpc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = LoggerServiceBuilder.Build();

            builder.Services.AddApplicationServices();
            builder.Services.AddInfraServices(builder.Configuration);
            // Add services to the container.
            builder.Services.AddGrpc(option =>
            {
                option.Interceptors.Add<ThreadCultureInterceptor>();
                option.EnableMessageValidation();
                option.Interceptors.Add<ExceptionHandlingInterceptor>();
            });
            builder.Services.AddExternalServices(builder.Configuration);
            builder.Services.AddAppValidators();

            builder.Host.UseSerilog();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDbContext>();
                context.Database.Migrate();
            }
            // Configure the HTTP request pipeline.
            app.MapGrpcService<AccountUsersQueriesService>();
            app.MapGrpcService<RebuildService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}