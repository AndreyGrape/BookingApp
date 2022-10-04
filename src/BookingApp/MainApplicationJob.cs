using BookingApp.Contracts;
using Microsoft.Extensions.Logging;
using Quartz;

namespace BookingApp
{
    public sealed class MainApplicationJob : IJob
    {
        private readonly ILogger<MainApplicationJob> _logger;
        private readonly IApplicationProcessing _applicationProcessing;

        public MainApplicationJob(ILogger<MainApplicationJob> logger, IApplicationProcessing applicationProcessing)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _applicationProcessing = applicationProcessing ?? throw new ArgumentNullException(nameof(applicationProcessing));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Job Started >>>>>");

            try
            {
                await Task.WhenAll(
                    Task.Run(() => Processing(EntityType.Client, "clients_change", context.CancellationToken)),
                    Task.Run(() => Processing(EntityType.Booking, "bookings_change", context.CancellationToken)),
                    Task.Run(() => Processing(EntityType.Event, "events_change", context.CancellationToken)));

                _logger.LogInformation("<<<<< Job Finished");
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }

        private async Task Processing(EntityType entityType, string key, CancellationToken cancellationToken = default)
        {
            var message = await _applicationProcessing.LookingForChange(entityType, cancellationToken);
            if (IsNotEmpty(message))
            {
                if (await _applicationProcessing.SendToQueue(message, cancellationToken))
                {
                    await _applicationProcessing.Safe2KeyValueStore(key, message, cancellationToken);
                }
            }
        }

        private static bool IsNotEmpty(string? item)
        {
            return !(item == null || item.Trim().Length == 0) && item != "[]";
        }
    }
}
