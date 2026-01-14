using gRPC_Server.Models.Filters;
using Microsoft.EntityFrameworkCore;

namespace gRPC_Server.Services;

public class FilterService
{
    public async Task<List<Product>> FilterProduct(ProductFilter filter, ApplicationDbContext context)
    {
        var query = context.Products.AsQueryable();

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

        return await query.ToListAsync<Product>();
    }
}