using BookingApp.Contracts;
using BookingApp.DataAccessLayer.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BookingApp.Services
{
    public sealed class ApplicationProcessing : IApplicationProcessing
    {
        private readonly ILogger<ApplicationProcessing> _logger;
        private readonly IBookingAppRepository _repository;
        private readonly IQueueProvider _queueProvider;
        private readonly IKeyValueStoreProvider _keyValueStoreProvider;

        public ApplicationProcessing(
            ILogger<ApplicationProcessing> logger,
            IBookingAppRepository repository,
            IKeyValueStoreProvider keyValueStoreProvider,
            IQueueProvider queueProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _keyValueStoreProvider = keyValueStoreProvider ?? throw new ArgumentNullException(nameof(keyValueStoreProvider));
            _queueProvider = queueProvider ?? throw new ArgumentNullException(nameof(queueProvider));
        }

        public async Task<string> LookingForChange(EntityType entityType, CancellationToken cancellationToken = default)
        {
            try
            {
                var ret = entityType switch
                {
                    EntityType.Client => JsonConvert.SerializeObject(await _repository.GetClientsChange(cancellationToken)),
                    EntityType.Booking => JsonConvert.SerializeObject(await _repository.GetBookingsChange(cancellationToken)),
                    EntityType.Event => JsonConvert.SerializeObject(await _repository.GetEventsChange(cancellationToken)),
                    _ => throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null)
                };

                _logger.LogInformation(ret);
                return ret;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<bool> SendToQueue(string message, CancellationToken cancellationToken = default)
        {
            return await _queueProvider.SendMessage2Queue(message, cancellationToken);
        }

        public async Task Safe2KeyValueStore(string key, string message, CancellationToken cancellationToken = default)
        {
            await _keyValueStoreProvider.SetKeyValueAsync(key, message, cancellationToken);
        }
    }
}
