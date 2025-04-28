using Amazon.SimpleNotificationService.Model;
using ASP_AWSTest.Response;
using System.Net;

namespace ASP_AWSTest.IService
{
    public interface ISNSService
    {
        Task<(Response<PublishResponse>, HttpStatusCode)> SendEmailAsync(string topicArn);
        Task<(Response<PublishResponse>, HttpStatusCode)> SendSMSAsync(string phNo);
    }
}
