using ASP_AWSTest.Model;

namespace ASP_AWSTest.Request;

public class UpdateCustomerRequest
{
    public string? CustomerId { get; set; }
    public string? OrderID { get; set; }
    public string? CustomerFirstName { get; set; }
    public string? CustomerLastName { get; set; }
    public DateTime? OrderDate { get; set; }
    public ItemDetails? ItemDetail { get; set; }
    public int Amount { get; set; }
}
