using Calzolari.Grpc.AspNetCore.Validation;

namespace Anis.AccountUsers.Queries.Grpc.Validatiors.Main
{
    public static class ValidationContainer
    {
        public static IServiceCollection AddAppValidators(this IServiceCollection services)
        {
            services.AddGrpcValidation();
            services.AddScoped<GrpcValidator>();
            services.AddValidator<GetAccountUsersRequestValidator>();

          

            return services;
        }
    }
}