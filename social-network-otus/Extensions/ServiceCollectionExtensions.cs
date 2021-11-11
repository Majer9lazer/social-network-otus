using Microsoft.Extensions.DependencyInjection;
using social_network_otus.Repositories.Dapper.MySql;
using social_network_otus.Services;

#if LOCAL
using RandomNameGeneratorLibrary;
#endif
namespace social_network_otus.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IConnectionStringFactory, MySqlConnectionStringFactory>()
                .AddSingleton<IUserRepository, MySqlUserRepository>()
                .AddRandomUserProvider();
        }

        private static IServiceCollection AddRandomUserProvider(this IServiceCollection services)
        {
#if LOCAL
            return services
                .AddSingleton<PersonNameGenerator>()
                .AddSingleton<PlaceNameGenerator>()
                .AddSingleton<IRandomUserProvider, RandomUserProvider>()
                ;
#else 
            return services.AddSingleton<IRandomUserProvider, FakeRandomUserProvider>();
#endif
        }
    }
}
