using gRPC_Server.Models.Filters;
using Microsoft.EntityFrameworkCore;

namespace gRPC_Server.Services;

public interface IProductService
{
    public Task<Get_Product_Response> GetProductByIdAsync(Get_Product_Request request);
    public Task<List<Get_All_Products_Response>> GetAllProductsAsync(Get_All_Products_Request request);
    public Task<Get_Product_Response> CreateProductAsync(Create_Product_Request request);
    public Task<Get_Product_Response> UpdateProductAsync(Update_Product_Request request);
    public Task<Delete_Message> DeleteProductAsync(Delete_Product_Request request);
}
