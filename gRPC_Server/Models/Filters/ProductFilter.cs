namespace gRPC_Server.Models.Filters;

public class ProductFilter
{
    public Int32? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double? Price { get; set; }
    public Int32? Stock_Quantity { get; set; }
}