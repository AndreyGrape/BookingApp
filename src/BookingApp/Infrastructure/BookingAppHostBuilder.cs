using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BookingApp.Infrastructure
{
    public static class BookingAppHostBuilder
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                    .UseConsoleLifetime()
                    .ConfigureAppConfiguration(configBuilder =>
                    {
                        configBuilder.AddJsonFile("appsettings.json", optional: true);
                    })
                    .ConfigureServices((context, services) =>
                    {
                        services.AddServices(context.Configuration);
                    });
    }
}
