using Anis.AccountUsers.Queries.Grpc.Protos.History;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Options;

namespace Anis.AccountUsers.Queries.Grpc.Setup
{
    public static class ExternalServicesRegistration
    {
        public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddServicesUrlsOptions(services, configuration);

            services.AddGrpcClient<AccountUsersEventHistory.AccountUsersEventHistoryClient>((provider, options) =>
            {
                RegisterUrl(provider, urls => urls.AccountUsersCommandUrl, options);
            });


            return services;
        }
        private static void AddServicesUrlsOptions(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<ExternalServiceOptions>()
                .Bind(configuration.GetSection(ExternalServiceOptions.ExternalServices))
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }

        private static void RegisterUrl(IServiceProvider provider, Func<ExternalServiceOptions, string> getUrl, GrpcClientFactoryOptions options)
        {
            var servicesUrls = provider.GetRequiredService<IOptions<ExternalServiceOptions>>().Value;
            options.Address = new Uri(getUrl(servicesUrls));
        }
    }
}
