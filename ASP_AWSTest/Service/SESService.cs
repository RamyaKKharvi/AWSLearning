using Amazon.SimpleEmail.Model;
using ASP_AWSTest.IService;
using ASP_AWSTest.Constant;
using ASP_AWSTest.Response;
using System.Net;
using Amazon.SimpleEmail;
using Amazon;

namespace ASP_AWSTest.Service;

public class SESService : ISESService
{
    public async Task<(Response<SendEmailResponse>, HttpStatusCode)> SendEmailAsync(string recipientEmail)
    {
        try
        {

            using var _sesClient = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast1);

            var emailRequest = new SendEmailRequest
            {
                Source = SESConst.senderEmail,
                Destination = new Destination()
                {
                    ToAddresses = new List<string>()
                        {
                            recipientEmail
                        }
                },
                Message = new Message
                {
                    Subject = new Content()
                    {
                        Charset = "UTF-8",
                        Data = SESConst.subject
                    },
                    Body = new Body()
                    {
                        Text = new Content()
                        {
                            Charset = "UTF-8",
                            Data = SESConst.description
                        },
                        Html = new Content()
                        {
                            Charset = "UTF-8",
                            Data = "Hello <b>Ramya</b>"
                        }
                    }
                }
            };

            var response = await _sesClient.SendEmailAsync(emailRequest);

            return (Response<SendEmailResponse>.SuccessResult(message: "Sent Email Successfully.", data: response), HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return (Response<SendEmailResponse>.FailureResult(message: ex.Message), HttpStatusCode.BadRequest);
        }
    }
}
