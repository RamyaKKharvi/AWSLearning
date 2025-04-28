using System.Net;
using Microsoft.AspNetCore.Mvc;
using ASP_AWSTest.IService;

namespace ASP_AWSTest.Controllers
{

    [Route("ses")]
    [ApiController]
    public class SESController : ControllerBase
    {
        private readonly ISESService _sesService;
        private readonly ILogger<SESController> _logger;

        public SESController(ISESService sesService, ILogger<SESController> logger)
        {
            _sesService = sesService;
            _logger = logger;
        }

        [HttpGet("send-email")]
        public async Task<ActionResult> SendEmailAsync(string recipientEmail)
        {
            try
            {
                _logger.LogInformation("Process started.");
                var (result, statusCode) = await _sesService.SendEmailAsync(recipientEmail);

                if (statusCode == HttpStatusCode.OK)
                {
                    _logger.LogInformation($"Status code: {statusCode}");
                    return StatusCode((int)statusCode, result);
                }
                _logger.LogWarning($"Status code: {statusCode}");
                return StatusCode((int)statusCode, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }

        }
    }
}
