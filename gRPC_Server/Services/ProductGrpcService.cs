using AutoMapper;
using Grpc.Core;

namespace gRPC_Server.Services;

public class ProductGrpcService(ProductService productService, IMapper mapper) : gRPC_Server.ProductService.ProductServiceBase
{
    public override async Task<Get_All_Products_Response> GetAllProducts(Get_All_Products_Request request, ServerCallContext context)
    {
        var listResponse = await productService.GetAllProductsAsync(request);
        var products = mapper.Map<List<Product>>(listResponse);
        var response = new Get_All_Products_Response();
        response.Products.AddRange(products);
        return response;
    }

    public override async Task<Get_Product_Response> CreateProduct(Create_Product_Request request, ServerCallContext context)
    {
        var creatProduct = await productService.CreateProductAsync(request);
        return creatProduct;
    }

    public override Task<Get_Product_Response> UpdateProduct(Update_Product_Request request, ServerCallContext context)
    {
        var updatedProduct = productService.UpdateProductAsync(request);
        return updatedProduct;
    }

    public override Task<Delete_Message> DeleteProduct(Delete_Product_Request request, ServerCallContext context)
    {
        var deletedMessage = productService.DeleteProductAsync(request);
        return deletedMessage;
    }

    public override Task<Get_Product_Response> GetProduct(Get_Product_Request request, ServerCallContext context)
    {
        var product = productService.GetProductByIdAsync(request);
        return product;
    }
}