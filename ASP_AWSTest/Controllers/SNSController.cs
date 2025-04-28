using System.Net;
using ASP_AWSTest.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP_AWSTest.Controllers;

[Route("sns")]
[ApiController]
public class SNSController(ISNSService snsService) : ControllerBase
{
    private readonly ISNSService _snsService = snsService;

    [HttpPost("send-sms")]
    public async Task<ActionResult> SendSnsSMSAsync(string phNo)
    {
        var (response, statusCode) = await _snsService.SendSMSAsync(phNo);
        return StatusCode((int)statusCode, response);
    }

    [HttpGet("send-email")]
    public async Task<ActionResult> SendSnsEmailAsync(string topicArn)
    {
        var (response, statusCode) = await _snsService.SendEmailAsync(topicArn);
        return StatusCode((int)statusCode, response);
    }
}

