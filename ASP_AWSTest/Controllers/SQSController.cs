using ASP_AWSTest.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP_AWSTest.Controllers
{
    [Route("sqs")]
    [ApiController]
    public class SQSController : ControllerBase
    {
        private readonly ISQSService _sqsService;
        public SQSController(ISQSService sqsService)
        {
            _sqsService = sqsService;
        }

        [HttpGet("queue")]

        public async Task<IActionResult> SendQueueAsync(string QueueUrl)
        {
            var (response, statusCode) = await _sqsService.SendQueueMessageAsync(QueueUrl);
            return StatusCode((int)statusCode, response);
        }
    }
}
