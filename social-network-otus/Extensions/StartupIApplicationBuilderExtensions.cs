using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace social_network_otus.Extensions
{
    public static class StartupIApplicationBuilderExtensions
    {
        public static IApplicationBuilder InitDbContext<TDbContext>(this IApplicationBuilder builder)
            where TDbContext : DbContext
        {
            using var scope = builder.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
            dbContext.Database.Migrate();
            return builder;
        }
    }
}
