using Amazon.SageMakerRuntime.Model;
using System.Text.Json;
using System.Text;
using ASP_AWSTest.IService;
using Amazon.SageMakerRuntime;

namespace ASP_AWSTest.Service;

public class SageMakerService(IAmazonSageMakerRuntime amazonSageMaker) : ISageMakerService
{
    private readonly IAmazonSageMakerRuntime _amazonSageMaker = amazonSageMaker;
    public async Task<string> CategorizeSagemakerTextAsync(string sagemakerText, string endpointName)
    {
        try
        {
            var jsonText = JsonSerializer.Serialize(sagemakerText);
            byte[] byteData = Encoding.UTF8.GetBytes(jsonText);
            InvokeEndpointRequest request = new InvokeEndpointRequest
            {
                EndpointName = endpointName,
                Accept = "application/json",
                ContentType = "application/json",
                Body = new MemoryStream(byteData)
            };
            var response = await _amazonSageMaker.InvokeEndpointAsync(request);
            var predictions = Encoding.UTF8.GetString(response.Body.ToArray());

            var result = JsonSerializer.Deserialize<string>(predictions);
            if (result != null)
            {
                return result;
            }
            throw new ArgumentNullException(result);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
