using Microsoft.Extensions.DependencyInjection;

namespace social_network_otus.Services
{
    public interface IServiceScopeFactory<TInstance>
    {
        TInstance ScopedInstance { get; }
    }

    public class ServiceScopeFactory<TInstance> : IServiceScopeFactory<TInstance>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ServiceScopeFactory(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;

        }

        public TInstance ScopedInstance => _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<TInstance>();
    }
}