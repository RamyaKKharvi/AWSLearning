using ASP_AWSTest.IService;
using Microsoft.AspNetCore.Mvc;

namespace ASP_AWSTest.Controllers
{
    [Route("sagemaker")]
    [ApiController]
    public class SageMakerController(ISageMakerService sageMakerService) : ControllerBase
    {
        private readonly ISageMakerService _sageMakerService = sageMakerService;
        [HttpGet]
        public async Task<ActionResult> CategorizeTextAsync(string text, string endpointName)
        {
            var result = _sageMakerService.CategorizeSagemakerTextAsync(text, endpointName);
            return Ok(result);
        }
    }
}
