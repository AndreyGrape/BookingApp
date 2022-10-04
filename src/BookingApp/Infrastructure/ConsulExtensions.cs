using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookingApp.Infrastructure
{
    public static class ConsulExtensions
    {
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(_ =>
                new ConsulClient(consulConfig =>
                {
                    var address = configuration.GetValue<string>("Consul");
                    consulConfig.Address = new Uri(address);
                }));
            return services;
        }
    }
}
