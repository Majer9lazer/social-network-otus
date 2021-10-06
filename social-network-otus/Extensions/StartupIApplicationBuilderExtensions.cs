﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace social_network_otus.Extensions
{
    public static class StartupIApplicationBuilderExtensions
    {
        public static IApplicationBuilder InitDbContext<TDbContext>(this IApplicationBuilder builder)
            where TDbContext : DbContext
        {
            using var scope = builder.ApplicationServices.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
            var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
            logger.LogInformation("DbContext connection string = {0}", dbContext.Database.GetConnectionString());
            dbContext.Database.Migrate();
            return builder;
        }
    }
}
