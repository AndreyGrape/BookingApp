using System.Net;
using Amazon.SQS;
using Amazon.SQS.Model;
using BookingApp.Contracts;
using Microsoft.Extensions.Logging;

namespace BookingApp.Providers
{
    public sealed class AmazonSqsProvider : IQueueProvider
    {
        private readonly ILogger<AmazonSqsProvider> _logger;
        private readonly IAmazonSQS _amazonSqs;
        private readonly IQueueBoxProvider _queueBox;

        public AmazonSqsProvider(
            ILogger<AmazonSqsProvider> logger,
            IQueueBoxProvider queueBox,
            IAmazonSQS amazonSqs)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _queueBox = queueBox ?? throw new ArgumentNullException(nameof(queueBox));
            _amazonSqs = amazonSqs ?? throw new ArgumentNullException(nameof(amazonSqs));
        }

        public async Task<bool> SendMessage2Queue(string message, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _amazonSqs.SendMessageAsync(
                    new SendMessageRequest(_queueBox.GetQueueUrl(), message),
                    cancellationToken);

                return response?.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
    }
}
