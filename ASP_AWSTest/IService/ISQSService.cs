using System.Net;
using Amazon.SQS.Model;
using ASP_AWSTest.Response;

namespace ASP_AWSTest.IService
{
    public interface ISQSService
    {
        Task<(Response<SendMessageResponse>, HttpStatusCode)> SendQueueMessageAsync(string queueUrl);
    }
}
