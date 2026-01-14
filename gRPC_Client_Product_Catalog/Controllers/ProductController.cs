using gRPC_Client;
using Microsoft.AspNetCore.Mvc;

namespace gRPC_Client_Product_Catalog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductService.ProductServiceClient _grpcClient;

    public ProductController(ProductService.ProductServiceClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
    {
        var response = await _grpcClient.GetAllProductsAsync(new Get_All_Products_Request());
        return Ok(response.Products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        var response = await _grpcClient.GetProductAsync(new Get_Product_Request { Id = id });
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Create_Product_Request request)
    {
        var response = await _grpcClient.CreateProductAsync(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<Product>> UpdateProduct(Update_Product_Request request)
    {
        var response = await _grpcClient.UpdateProductAsync(request);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> DeleteProduct(int id)
    {
        var response = await _grpcClient.DeleteProductAsync(new Delete_Product_Request { Id = id });
        return Ok(response.Message);
    }
}