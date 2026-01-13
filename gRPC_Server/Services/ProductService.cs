using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace gRPC_Server.Services;

public class ProductService(ILogger<ProductService> _logger,ApplicationDbContext _context,IMapper _mapper) : IProductService
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

    public Task<Get_All_Products_Response> GetAllProductsAsync(Get_All_Products_Request request)
    {
        var query = _context.Products.AsQueryable();

        if (request.Id.HasValue)
        {
            query = query.Where(x => x.Id == request.Id.Value);
        }
        if (!string.IsNullOrEmpty(request.Message))
        {
            query = query.Where(x => x.Message.Contains(request.Message));
        }
        if (request.CreatedAt.HasValue)
        {
            query = query.Where(x => x.CreatedAt == request.CreatedAt.Value);
        }
        if (request.InRead.HasValue)
        {
            query = query.Where(x => x.InRead == request.InRead.Value);
        }
        if (request.Expired.HasValue)
        {
            query = query.Where(x => x.Expired == request.Expired.Value);
        }
        if (request.NotificationId.HasValue)
        {
            query = query.Where(x => x.NotificationId == request.NotificationId.Value);
        }
        var todoitem = await query.ToListAsync();
        cachedResult = mapper.Map<List<UserNotifDto>>(todoitem);

        _cache.Set(cacheKey, cachedResult);
    }

    return new Response<List<UserNotifDto>>
    {
        StatusCode = (int)HttpStatusCode.OK,
        Message = "User Notification retrieved successfully!",
        Data = cachedResult
    };
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