using BookingApp.DataAccessLayer.Contracts;
using BookingApp.DataAccessLayer.Dapper;
using Microsoft.Extensions.DependencyInjection;

namespace BookingApp.DataAccessLayer.Extensions.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDalRepository(this IServiceCollection services)
        {
            services
                .AddTransient<BookingDbContext>()
                .AddTransient<IBookingAppRepository, BookingAppRepository>();
            return services;
        }
    }
}
