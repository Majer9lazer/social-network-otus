using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(social_network_otus.Areas.Identity.IdentityHostingStartup))]
namespace social_network_otus.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}