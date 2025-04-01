using Amazon.SimpleEmail.Model;
using ASP_AWSTest.Response;
using System.Net;

namespace ASP_AWSTest.IService;

public interface ISESService 
{
    Task<(Response<SendEmailResponse>, HttpStatusCode)> SendEmailAsync(string recipientEmail);
}