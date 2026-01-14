using gRPC_Server.Services;
using Microsoft.EntityFrameworkCore;
using Infastructure.AutoMapper;
using Serilog;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

/*/ Настройка Serilog
Log.Logger = new LoggerConfiguration() 
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();*/

// Add services to the container.
builder.Services.AddGrpc();

// In-memory database
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(connection));
// AutoMapper registration
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

// Register application services
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Подключение твоего кастомного middleware
app.Use(async (context, next) =>
{
    // Пример: логируем каждый запрос
    Log.Information("Handling request: {Path}", context.Request.Path);

    await next.Invoke();

    Log.Information("Finished request: {Path}", context.Request.Path);
});

// Configure the HTTP request pipeline.
app.MapGrpcService<ProductGrpcService>();
app.MapGet("/", () =>
    "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();