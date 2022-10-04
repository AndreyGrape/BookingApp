using BookingApp.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookingApp
{
    public class Program
    {
        static async Task<int> Main(string[] args)
        {
            using var host = BookingAppHostBuilder
                .CreateHostBuilder(args)
                .Build();

            using var scope = host.Services.CreateScope();

            var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<Program>();

            await host.RunAsync();
            logger.LogInformation("Main: Application has completed");

            return 0;
        }
    }
}
