using Microsoft.Extensions.DependencyInjection;
using RandomNameGeneratorLibrary;
using social_network_otus.Repositories.Dapper.MySql;
using social_network_otus.Services;

namespace social_network_otus.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<PersonNameGenerator>()
                .AddSingleton<PlaceNameGenerator>()
                .AddSingleton<IRandomUserProvider, RandomUserProvider>()
                .AddSingleton<IConnectionStringFactory, MySqlConnectionStringFactory>()
                .AddSingleton<IUserRepository, MySqlUserRepository>();
        }
    }
}
