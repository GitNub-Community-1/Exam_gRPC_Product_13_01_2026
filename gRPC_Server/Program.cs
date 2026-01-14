using gRPC_Server.Services;
using Microsoft.EntityFrameworkCore;
using Infastructure.AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

// Use an in-memory database for development so no connection string is required.
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("ProductsDb"));

// AutoMapper registration
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

// Register application services
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ProductGrpcService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();