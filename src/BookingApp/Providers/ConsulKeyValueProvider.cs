using Consul;
using System.Text;
using BookingApp.Contracts;
using Microsoft.Extensions.Logging;

namespace BookingApp.Providers
{
    public sealed class ConsulKeyValueProvider : IKeyValueStoreProvider
    {
        private readonly ILogger<ConsulKeyValueProvider> _logger;
        private readonly IConsulClient _consulClient;

        public ConsulKeyValueProvider(
            ILogger<ConsulKeyValueProvider> logger,
            IConsulClient consulClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _consulClient = consulClient ?? throw new ArgumentNullException(nameof(consulClient));
        }

        public async Task SetKeyValueAsync(string key, string consulValue, CancellationToken cancellationToken = default)
        {
            try
            {
                var kvPair = new KVPair(key)
                {
                    Value = Encoding.UTF8.GetBytes(consulValue)
                };

                await _consulClient.KV.Put(kvPair, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
    }
}
