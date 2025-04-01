
namespace ASP_AWSTest.IService;

public interface ISageMakerService
{
    Task<string> CategorizeSagemakerTextAsync(string sagemakerText, string endpointName);
}
