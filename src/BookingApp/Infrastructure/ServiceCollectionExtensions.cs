using BookingApp.Contracts;
using BookingApp.Services;
using BookingApp.Infrastructure.Quartz;
using BookingApp.DataAccessLayer.Extensions.Infrastructure;
using BookingApp.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using LocalStack.Client.Extensions;
using Amazon.SQS;

namespace BookingApp.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDalRepository()

                .AddSingleton<IJobFactory, JobFactory>()
                .AddSingleton<ISchedulerFactory, StdSchedulerFactory>()
                .AddSingleton<QuartzJobRunner>()
                .AddHostedService<QuartzHostedService>()

                .AddTransient<MainApplicationJob>()
                .AddSingleton(new JobParamsWrapper(
                    jobType: typeof(MainApplicationJob),
                    cronExpression: configuration.GetSection("CronExpressions")?[nameof(MainApplicationJob)] ?? string.Empty))

                .AddLocalStack(configuration)
                .AddDefaultAWSOptions(configuration.GetAWSOptions())
                .AddAwsService<IAmazonSQS>()

                .AddConsulConfig(configuration)

                .AddTransient<IKeyValueStoreProvider, ConsulKeyValueProvider>()
                .AddTransient<IQueueProvider, AmazonSqsProvider>()
                .AddSingleton<IQueueBoxProvider, QueueBoxProvider>()

                .AddSingleton<ChangeVersion>()
                .AddTransient<IApplicationProcessing, ApplicationProcessing>();

            return services;
        }
    }
}
