using Amazon.SQS;
using ASP_AWSTest.Response;
using ASP_AWSTest.Constant;
using ASP_AWSTest.IService;
using System.Net;
using Amazon.SQS.Model;

namespace ASP_AWSTest.Service;

public class SQSService : ISQSService
{
    private readonly IAmazonSQS _amazonSQS;
    private readonly ILogger<SQSService> _logger;
    public SQSService(IAmazonSQS amazonSQS, ILogger<SQSService> logger)
    {
        _amazonSQS = amazonSQS;
        _logger = logger;
    }
    public async Task<(Response<SendMessageResponse>, HttpStatusCode)> SendQueueMessageAsync(string queueUrl)
    {
        try
        {
            _logger.LogInformation("SQS process started.");
            var messageRequest = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = SQSConst.Message + DateTime.Now.ToLongDateString()
            };

            var response = await _amazonSQS.SendMessageAsync(messageRequest);
            _logger.LogInformation($"SQS successfull: {response}");
            return (Response<SendMessageResponse>.SuccessResult(message: "Queue successfull!", data: response), HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return (Response<SendMessageResponse>.FailureResult(ex.Message), HttpStatusCode.InternalServerError);
        }
    }
}
