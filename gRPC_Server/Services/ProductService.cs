using System.Net;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using gRPC_Server.Models.Filters;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace gRPC_Server.Services;

public class ProductService(ILogger<ProductService> _logger, ApplicationDbContext _context, IMapper _mapper)
    : IProductService
{
    public async Task<Get_Product_Response> GetProductByIdAsync(Get_Product_Request request)
    {
        _logger.LogInformation("Received request for ProductId: {UserId}", request.Id);
        var product = await _context.Products.FindAsync(request.Id);

        if (product == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
        }

        var response = _mapper.Map<Get_Product_Response>(product);
        _logger.LogInformation("Returning Product: {Name}", response.Name);
        return response;
    }

    public async Task<List<Get_All_Products_Response>> GetAllProductsAsync(Get_All_Products_Request request)
    {
        var filter = _mapper.Map<ProductFilter>(request);
        var query = _context.Products.AsQueryable();

        if (filter.Id.HasValue)
        {
            query = query.Where(x => x.Id == filter.Id.Value);
        }

        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(x => x.Name.Contains(filter.Name));
        }

        if (!string.IsNullOrEmpty(filter.Description))
        {
            query = query.Where(x => x.Description.Contains(filter.Description));
        }

        if (filter.Price.HasValue)
        {
            query = query.Where(x => x.Price == filter.Price.Value);
        }

        if (filter.Stock_Quantity.HasValue)
        {
            query = query.Where(x => x.StockQuantity == filter.Stock_Quantity.Value);
        }

        var products = await query.ToListAsync();
        return _mapper.Map<List<Get_All_Products_Response>>(products);
    }

    

    public async Task<Get_Product_Response> CreateProductAsync(Create_Product_Request request)
    {
        var product = _mapper.Map<Product>(request);
        product.Id = 0;
        _logger.LogInformation("Create request for Pser: {Name}", product.Name);
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Added Product with Id: {Id}", product.Id);
        return _mapper.Map<Get_Product_Response>(product);
    }

    public async Task<Get_Product_Response> UpdateProductAsync(Update_Product_Request request)
    {
        var product = await _context.Products.FindAsync(request.Id);
        if (product == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
        
        _mapper.Map(request, product);
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated product with Id: {Id}", product.Id);
        return _mapper.Map<Get_Product_Response>(product);
    }

    public async Task<Delete_Message> DeleteProductAsync(Delete_Product_Request request)
    {
        var product = await _context.Products.FindAsync(request.Id);
        if (product == null)
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted product with Id: {Id}", product.Id);
        return new Delete_Message(){Message = $"Product Deleted, Id = {product.Id}, Name = {product.Name}"};
    }
}