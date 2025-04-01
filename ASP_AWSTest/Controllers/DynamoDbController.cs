using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using ASP_AWSTest.IService;
using ASP_AWSTest.Request;
using Microsoft.AspNetCore.Mvc;

namespace ASP_AWSTest.Controllers
{
    [Route("dynamodb/customer")]
    [ApiController]
    public class DynamoDbController : ControllerBase
    {
        private readonly IDynamoDbService _dynamoService;
        private readonly DynamoDBContext _dynamoDBContext;
        public DynamoDbController(IAmazonDynamoDB dynamoDB, IDynamoDbService dbService, IAmazonDynamoDB amazonDynamoDB)
        {
            _dynamoService = dbService;
            _dynamoDBContext = new DynamoDBContext(amazonDynamoDB);
        }
        [HttpGet("orders")]
        public async Task<ActionResult> GetAllCustomersOrderDetailsAsync()
        {
            var (response, statusCode) = await _dynamoService.GetAllCustomersOrderListAsync();
            return StatusCode((int)statusCode, response);
        }

        [HttpPost("order/create")]
        public async Task<ActionResult> CreateCustomerOrderAsync(CustomerRequest request)
        {
            var (response, statusCode) = await _dynamoService.AddCustomerOrderAsync(request);
            return StatusCode((int)statusCode, response);
        }

        [HttpGet("{customerId}")]
        public async Task<ActionResult> GetCustomerOrdersByIdAsync(string customerId)
        {
            var (response, statusCode) = await _dynamoService.GetCustomerAllOrdersByCustomerIdAsync(customerId);
            return StatusCode((int)statusCode, response);
        }

        [HttpGet("order")]
        public async Task<ActionResult> GetCustomerOrderByOrderIdAsync(string customerId, string orderId)
        {
            var (response, statusCode) = await _dynamoService.GetCustomerOrderByOrderIdAsync(customerId, orderId);
            return StatusCode((int)statusCode, response);
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateCustomerOrderDetailAsync([FromQuery] UpdateCustomerRequest request)
        {
            var (response, statusCode) = await _dynamoService.UpdateCustomerOrderDetailAsync(request);
            return StatusCode((int)statusCode, response);
        }

        [HttpPatch("update/partial")]
        public async Task<ActionResult> PartialUpdateCustomerOrderDetailAsync(string customerId, string orderId, string firstName, string lastName)
        {
            var (response, statusCode) = await _dynamoService.PartialUpdateCustomerOrderDetailAsync(customerId, orderId, firstName, lastName);
            return StatusCode((int)statusCode, response);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteCustomerByCustomerIdAsync(string customerId, string orderId)
        {
            var (response, statusCode) = await _dynamoService.DeleteCustomerAsync(customerId, orderId);
            return StatusCode((int)statusCode, response);
        }
    }
}
