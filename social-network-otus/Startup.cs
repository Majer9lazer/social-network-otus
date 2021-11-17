using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using social_network_otus.Data;
using social_network_otus.Data.Models;
using social_network_otus.Extensions;
using social_network_otus.Hubs;
using System;

namespace social_network_otus
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServices();

            services.AddDbContextPool<ApplicationDbContext>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("MySqlConnection"));

                var provider = services.BuildServiceProvider();
                var loggerFactory = provider.GetRequiredService<ILoggerFactory>();

                options
                    .UseLoggerFactory(loggerFactory)
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging();
            });
            services.AddDatabaseDeveloperPageExceptionFilter();
            services
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
                //options.MaximumParallelInvocationsPerClient = 10;
                options.MaximumReceiveMessageSize = 4096;
                options.ClientTimeoutInterval = TimeSpan.FromDays(1);
                options.HandshakeTimeout = TimeSpan.FromHours(12);
            });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Local"))
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.InitDbContext<ApplicationDbContext>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
                endpoints.MapHub<ProfileHub>("/profileHub");
            });
        }
    }
}
