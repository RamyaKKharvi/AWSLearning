using System.Net;
using Amazon.DynamoDBv2.Model;
using ASP_AWSTest.Model;
using ASP_AWSTest.Request;
using ASP_AWSTest.Response;

namespace ASP_AWSTest.IService;

public interface IDynamoDbService
{
    Task<(Response<List<CustomerOrderDetails>>, HttpStatusCode)> GetAllCustomersOrderListAsync();
    Task<(Response<List<CustomerOrderDetails>>, HttpStatusCode)> GetCustomerAllOrdersByCustomerIdAsync(string customerId);
    Task<(Response<CustomerOrderDetails>, HttpStatusCode)> AddCustomerOrderAsync(CustomerRequest request);
    Task<(Response<CustomerOrderDetails>, HttpStatusCode)> UpdateCustomerOrderDetailAsync(UpdateCustomerRequest request);
    Task<(Response<CustomerOrderDetails>, HttpStatusCode)> DeleteCustomerAsync(string customerId, string orderId);
    Task<(Response<CustomerOrderDetails>, HttpStatusCode)> GetCustomerOrderByOrderIdAsync(string customerId, string orderId);
    Task<(Response<CustomerOrderDetails>, HttpStatusCode)> PartialUpdateCustomerOrderDetailAsync(string customerId, string orderId, string firstName, string lastName);
}
