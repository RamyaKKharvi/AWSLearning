using Amazon.SQS;
using Amazon.SQS.Model;

namespace AWS_EC_WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IAmazonSQS _amazonSQS;
        private string _queueUrl = "https://sqs.us-east-1.amazonaws.com/370633633388/UdemyTestQueue";

        public Worker(ILogger<Worker> logger, IAmazonSQS amazonSQS)
        {
            _logger = logger;
            _amazonSQS = amazonSQS;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //if (_logger.IsEnabled(LogLevel.Information))
                //{
                //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //}
                var request = new ReceiveMessageRequest
                {
                    QueueUrl = _queueUrl,
                    WaitTimeSeconds = 10,
                    MaxNumberOfMessages = 10,
                };

                var messages = await _amazonSQS.ReceiveMessageAsync(request);
                if (messages.Messages.Any())
                {

                    foreach (var message in messages.Messages)
                    {
                        _logger.LogInformation(message.Body);
                        await _amazonSQS.DeleteMessageAsync(_queueUrl, message.ReceiptHandle);
                    }
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
