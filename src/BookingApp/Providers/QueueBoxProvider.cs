using Amazon.SQS;
using Amazon.SQS.Model;
using BookingApp.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BookingApp.Providers
{
    public sealed class QueueBoxProvider : IQueueBoxProvider
    {
        private static readonly object Obj = new();

        private readonly ILogger<QueueBoxProvider> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAmazonSQS _amazonSqs;

        private string? _queueUrl;

        public QueueBoxProvider(
            ILogger<QueueBoxProvider> logger,
            IConfiguration configuration,
            IAmazonSQS amazonSqs)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _amazonSqs = amazonSqs ?? throw new ArgumentNullException(nameof(amazonSqs));
        }

        public string GetQueueUrl()
        {
            try
            {
                lock (Obj)
                {
                    if (_queueUrl == null || _queueUrl.Trim().Length == 0)
                    {
                        _queueUrl = CreateQueue();
                    }
                }

                return _queueUrl ?? string.Empty;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        private string CreateQueue()
        {
            var queueUrlName = _configuration.GetSection("QueueUrl")?.Value;
            var ret = _amazonSqs.CreateQueueAsync(new CreateQueueRequest(queueUrlName)).GetAwaiter().GetResult();

            return ret.QueueUrl;
        }
    }
}
