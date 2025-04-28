using Amazon.DynamoDBv2.DataModel;

namespace ASP_AWSTest.Model;

[DynamoDBTable("t_test")]
public class CustomerOrderDetails
{
    [DynamoDBHashKey]
    public string? CustomerId { get; set; }

    [DynamoDBRangeKey]
    public string? OrderID { get; set; }
    public string? CustomerName { get; set; }
    public DateTime? OrderDate { get; set; }
    [DynamoDBProperty]
    public ItemDetails? ItemDetail { get; set; }
    public int? Amount { get; set; }

    [DynamoDBProperty("Created_datetime")]
    public DateTime CreatedDateTime { get; set; }
    
    [DynamoDBProperty("Modified_datetime")]
    public DateTime ModifiedDateTime { get; set; }
}

public class ItemDetails
{
    public string? Name { get; set; }
    public int? Quantity { get; set; }
    public string? ItemType { get; set; }
    public string? ItemSize { get; set; }
    public string? Color { get; set; }
    public string? Material { get; set; }
    public string? Brand { get; set; }
    public string? Description { get; set; }
}

