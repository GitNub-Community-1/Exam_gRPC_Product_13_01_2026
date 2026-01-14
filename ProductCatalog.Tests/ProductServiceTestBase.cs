using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using gRPC_Server;
using gRPC_Server.Services;
using Infastructure.AutoMapper;

namespace ProductCatalog.Tests;

public abstract class ProductServiceTestBase
{
    protected readonly ApplicationDbContext Context;
    protected readonly IMapper Mapper;
    protected readonly Mock<ILogger<gRPC_Server.Services.ProductService>> LoggerMock;

    protected ProductServiceTestBase()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new ApplicationDbContext(options);
        Context.Database.EnsureCreated();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MapperProfile>();
        });

        Mapper = mapperConfig.CreateMapper();

        LoggerMock = new Mock<ILogger<gRPC_Server.Services.ProductService>>();
    }

    protected gRPC_Server.Services.ProductService CreateService()
        => new gRPC_Server.Services.ProductService(LoggerMock.Object, Context, Mapper);
}
