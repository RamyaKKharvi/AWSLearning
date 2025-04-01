using System.Collections.Generic;
using System.Net;
using System.Xml.Linq;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime.Internal;
using ASP_AWSTest.IService;
using ASP_AWSTest.Model;
using ASP_AWSTest.Request;
using ASP_AWSTest.Response;

namespace ASP_AWSTest.Service
{
    public class DynamoDbService(ILogger<DynamoDbService> logger, IAmazonDynamoDB dynamoDBClient) : IDynamoDbService
    {
        private readonly DynamoDBContext _dynamoDBContext = new DynamoDBContext(dynamoDBClient);
        private readonly ILogger<DynamoDbService> _logger = logger;

        public async Task<(Response<List<CustomerOrderDetails>>, HttpStatusCode)> GetAllCustomersOrderListAsync()
        {
            try
            {
                var scanConditions = new List<ScanCondition>();
                var result = await _dynamoDBContext.ScanAsync<CustomerOrderDetails>(scanConditions).GetRemainingAsync();
                if (result.Count == 0)
                {
                    return (Response<List<CustomerOrderDetails>>.FailureResult("No data found"), HttpStatusCode.NotFound);
                }
                return (Response<List<CustomerOrderDetails>>.SuccessResult(result), HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return (Response<List<CustomerOrderDetails>>.FailureResult(ex.Message), HttpStatusCode.BadRequest);
            }
        }

        public async Task<(Response<List<CustomerOrderDetails>>, HttpStatusCode)> GetCustomerAllOrdersByCustomerIdAsync(string customerId)
        {
            try
            {
                var result = await _dynamoDBContext.QueryAsync<CustomerOrderDetails>(customerId).GetRemainingAsync();
                if (result.Count == 0)
                {
                    return (Response<List<CustomerOrderDetails>>.FailureResult("No data found"), HttpStatusCode.NotFound);
                }
                return (Response<List<CustomerOrderDetails>>.SuccessResult(result), HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                return (Response<List<CustomerOrderDetails>>.FailureResult(ex.Message), HttpStatusCode.BadRequest);
            }
        }

        public async Task<(Response<CustomerOrderDetails>, HttpStatusCode)> GetCustomerOrderByOrderIdAsync(string customerId, string orderId)
        {
            try
            {
                var result = await _dynamoDBContext.LoadAsync<CustomerOrderDetails>(customerId, orderId);
                if (result == null)
                {
                    return (Response<CustomerOrderDetails>.FailureResult("No data found"), HttpStatusCode.NotFound);
                }
                return (Response<CustomerOrderDetails>.SuccessResult(result), HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return (Response<CustomerOrderDetails>.FailureResult(ex.Message), HttpStatusCode.BadRequest);
            }
        }

        public async Task<(Response<CustomerOrderDetails>, HttpStatusCode)> AddCustomerOrderAsync(CustomerRequest request)
        {
            try
            {
                var result = new CustomerOrderDetails
                {
                    CustomerId = "CUST#" + Guid.NewGuid().ToString(),
                    OrderID = "ORDER#" + DateOnly.FromDateTime(DateTime.UtcNow) + "#" + Guid.NewGuid().ToString()[^12..],
                    CustomerName = request.CustomerFirstName + " " + request.CustomerLastName,
                    OrderDate = request.OrderDate,
                    ItemDetail = new ItemDetails
                    {
                        Name = request.ItemDetail.Name,
                        Quantity = request.ItemDetail.Quantity,
                        ItemType = request.ItemDetail.ItemType,
                        ItemSize = request.ItemDetail.ItemSize,
                        Color = request.ItemDetail.Color,
                        Material = request.ItemDetail.Material,
                        Brand = request.ItemDetail.Brand,
                        Description = request.ItemDetail.Description
                    },
                    Amount = request.ItemDetail.Quantity * request.Amount,
                    CreatedDateTime = DateTime.UtcNow
                };
                await _dynamoDBContext.SaveAsync(result);
                return (Response<CustomerOrderDetails>.SuccessResult(result, $"Customer order created successfully CustomerId: {result.CustomerId}"), HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return (Response<CustomerOrderDetails>.FailureResult(ex.Message), HttpStatusCode.BadRequest);
            }
        }

        public async Task<(Response<CustomerOrderDetails>, HttpStatusCode)> UpdateCustomerOrderDetailAsync(UpdateCustomerRequest request)
        {
            try
            {
                var customerOrder = await _dynamoDBContext.LoadAsync<CustomerOrderDetails>(request.CustomerId, request.OrderID);
                if (customerOrder != null)
                {
                    var updatedData = new CustomerOrderDetails
                    {
                        CustomerId = request.CustomerId ?? throw new ArgumentNullException("CustomerId cannot be null."),
                        OrderID = request.OrderID ?? throw new ArgumentNullException("OrderId cannot be null."),
                        CustomerName = request.CustomerFirstName + " " + request.CustomerLastName ?? throw new ArgumentNullException("CustomerName or CustomerLastName cannot be null."),
                        OrderDate = request.OrderDate ?? throw new ArgumentNullException("Order date cannot be null."),
                        ItemDetail = new ItemDetails
                        {
                            Name = request.ItemDetail.Name ?? throw new ArgumentNullException("Item name cannot be null."),
                            Quantity = request.ItemDetail.Quantity ?? throw new ArgumentNullException("Provide quantity."),
                            ItemType = request.ItemDetail.ItemType ?? throw new ArgumentNullException("Item type cannot be null."),
                            ItemSize = request.ItemDetail.ItemSize ?? throw new ArgumentNullException("Item size cannot be null."),
                            Color = request.ItemDetail.Color ?? throw new ArgumentNullException("Item color cannot be null."),
                            Material = request.ItemDetail.Material ?? throw new ArgumentNullException("Item material cannot be null."),
                            Brand = request.ItemDetail.Brand ?? throw new ArgumentNullException("Item brand cannot be null."),
                            Description = request.ItemDetail.Description ?? throw new ArgumentNullException("Item description cannot be null.")
                        },
                        Amount = request.ItemDetail.Quantity * request.Amount ?? throw new ArgumentNullException("Provide amount"),
                        CreatedDateTime = customerOrder.CreatedDateTime,
                        ModifiedDateTime = DateTime.UtcNow
                    };
                    await _dynamoDBContext.SaveAsync(updatedData);
                    return (Response<CustomerOrderDetails>.SuccessResult(updatedData, "Customer order updated sucessfully."), HttpStatusCode.OK);
                }
                return (Response<CustomerOrderDetails>.FailureResult($"Customer with CustomerId: {request.CustomerId} and Order Id {request.OrderID} not found."), HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return (Response<CustomerOrderDetails>.FailureResult(ex.Message), HttpStatusCode.BadRequest);
            }
        }

        public async Task<(Response<CustomerOrderDetails>, HttpStatusCode)> PartialUpdateCustomerOrderDetailAsync(string customerId, string orderId, string firstName, string lastName)
        {
            try
            {
                var customerOrder = await _dynamoDBContext.LoadAsync<CustomerOrderDetails>(customerId, orderId);
                if (customerOrder != null)
                {
                    var updatedData = new CustomerOrderDetails
                    {
                        CustomerId = customerId ?? throw new ArgumentNullException("CustomerId cannot be null."),
                        OrderID = orderId ?? throw new ArgumentNullException("OrderId cannot be null."),
                        CustomerName = (firstName + " " + lastName) ?? customerOrder.CustomerName,
                        OrderDate = customerOrder.OrderDate,
                        ItemDetail = new ItemDetails
                        {
                            Name = customerOrder.ItemDetail.Name,
                            Quantity = customerOrder.ItemDetail.Quantity,
                            ItemType = customerOrder.ItemDetail.ItemType,
                            ItemSize = customerOrder.ItemDetail.ItemSize,
                            Color = customerOrder.ItemDetail.Color,
                            Material = customerOrder.ItemDetail.Material,
                            Brand = customerOrder.ItemDetail.Brand,
                            Description = customerOrder.ItemDetail.Description
                        },
                        Amount = customerOrder.Amount,
                        CreatedDateTime = customerOrder.CreatedDateTime,
                        ModifiedDateTime = DateTime.UtcNow
                    };

                    await _dynamoDBContext.SaveAsync(updatedData);
                    return (Response<CustomerOrderDetails>.SuccessResult(updatedData, "Customer order updated partially."), HttpStatusCode.OK);
                }
                return (Response<CustomerOrderDetails>.FailureResult($"Customer with CustomerId: {customerId} and Order Id {orderId} not found."), HttpStatusCode.NotFound);
            }
            catch (ArgumentNullException ex)
            {
                return (Response<CustomerOrderDetails>.FailureResult(ex.Message), HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return (Response<CustomerOrderDetails>.FailureResult(ex.Message), HttpStatusCode.BadRequest);
            }
        }

        public async Task<(Response<CustomerOrderDetails>, HttpStatusCode)> DeleteCustomerAsync(string customerId, string orderId)
        {
            try
            {
                var customerData = await _dynamoDBContext.LoadAsync<CustomerOrderDetails>(customerId, orderId);
                if (customerData != null)
                {
                    await _dynamoDBContext.DeleteAsync<CustomerOrderDetails>(customerId, orderId);
                    return (Response<CustomerOrderDetails>.SuccessResult(customerData, $"Deleted customer with customer Id {customerId} and Order Id {orderId} successfully."), HttpStatusCode.OK);
                }
                return (Response<CustomerOrderDetails>.FailureResult($"Customer with CustomerId: {customerId} and Order Id {orderId} not found."), HttpStatusCode.NotFound);

            }
            catch (Exception ex)
            {
                return (Response<CustomerOrderDetails>.FailureResult(ex.Message), HttpStatusCode.BadRequest);
            }
        }
    }
}