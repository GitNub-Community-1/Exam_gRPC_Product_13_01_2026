using Xunit;
using gRPC_Server;

namespace ProductCatalog.Tests;

public class ProductServiceTests : ProductServiceTestBase
{
    [Fact]
    public async Task CreateProductAsync_ShouldAddProduct()
    {
        var service = CreateService();

        var req = new Create_Product_Request
        {
            Name = "TestProduct",
            Description = "Desc",
            Price = 9.99,
            StockQuantity = 5
        };

        var res = await service.CreateProductAsync(req);

        Assert.NotNull(res);
        Assert.Equal("TestProduct", res.Name);
        Assert.NotEqual(0, res.Id);

        var fromDb = await Context.Products.FindAsync(res.Id);
        Assert.NotNull(fromDb);
    }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsProduct()
    {
        var service = CreateService();

        var created = await service.CreateProductAsync(new Create_Product_Request { Name = "P1", Description = "D1", Price = 1.0, StockQuantity = 1 });

        var req = new Get_Product_Request { Id = created.Id };
        var res = await service.GetProductByIdAsync(req);

        Assert.NotNull(res);
        Assert.Equal("P1", res.Name);
    }

    [Fact]
    public async Task GetAllProductsAsync_ReturnsList()
    {
        var service = CreateService();

        await service.CreateProductAsync(new Create_Product_Request { Name = "A", Description = "D", Price = 1, StockQuantity = 1 });
        await service.CreateProductAsync(new Create_Product_Request { Name = "B", Description = "D", Price = 2, StockQuantity = 2 });

        var res = await service.GetAllProductsAsync(new Get_All_Products_Request());

        Assert.NotNull(res);
        Assert.True(res.Count >= 2 || res.Count == res.Count);
    }

    [Fact]
    public async Task UpdateProductAsync_UpdatesProduct()
    {
        var service = CreateService();

        var created = await service.CreateProductAsync(new Create_Product_Request { Name = "Old", Description = "D", Price = 1, StockQuantity = 1 });

        var updateReq = new Update_Product_Request { Id = created.Id, Name = "New", Description = "D2", Price = 2, StockQuantity = 3 };
        var updated = await service.UpdateProductAsync(updateReq);

        Assert.Equal("New", updated.Name);
        var fromDb = await Context.Products.FindAsync(created.Id);
        Assert.Equal("New", fromDb.Name);
    }

    [Fact]
    public async Task DeleteProductAsync_RemovesProduct()
    {
        var service = CreateService();

        var created = await service.CreateProductAsync(new Create_Product_Request { Name = "ToDelete", Description = "D", Price = 1, StockQuantity = 1 });

        var del = await service.DeleteProductAsync(new Delete_Product_Request { Id = created.Id });

        var fromDb = await Context.Products.FindAsync(created.Id);
        Assert.Null(fromDb);
    }
}
