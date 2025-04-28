using System.Net;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using ASP_AWSTest.Constant;
using ASP_AWSTest.IService;
using ASP_AWSTest.Response;
using Microsoft.Extensions.Logging;

namespace ASP_AWSTest.Service;

public class SNSService : ISNSService
{
    private readonly IAmazonSimpleNotificationService _amazonSnsService;
    private readonly ILogger<SNSService> _logger;
    public SNSService(IAmazonSimpleNotificationService amazonSimpleNotificationService, ILogger<SNSService> logger)
    {
        _amazonSnsService = amazonSimpleNotificationService;
        _logger = logger;
    }
    public async Task<(Response<PublishResponse>, HttpStatusCode)> SendSMSAsync(string phNo)
    {
        try
        {
            _logger.LogInformation("SNS send SMS process started.");
            var publishRequest = new PublishRequest
            {
                PhoneNumber = phNo,
                Message = SNSConstant.TextMessage + DateTime.Now.ToLongDateString(),
            };

            var response = await _amazonSnsService.PublishAsync(publishRequest);
            _logger.LogInformation("SMS sent to phone.");

            return (Response<PublishResponse>.SuccessResult(message:$"SMS sent to {phNo}.", data:response), HttpStatusCode.OK);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return (Response<PublishResponse>.FailureResult(ex.Message), HttpStatusCode.InternalServerError);
        }
    }

    public async Task<(Response<PublishResponse>, HttpStatusCode)> SendEmailAsync(string topicArn)
    {
        try
        {
            var publishRequest = new PublishRequest
            {
                TopicArn = topicArn,
                Message = SNSConstant.EmailMessage + DateTime.Now.ToLongDateString(),
                Subject = SNSConstant.EmailSubject
            };

            var response = await _amazonSnsService.PublishAsync(publishRequest);

            return (Response<PublishResponse>.SuccessResult(message: "Email sent successfully.", data: response), HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return (Response<PublishResponse>.FailureResult(ex.Message), HttpStatusCode.InternalServerError);
        }
    }
}
