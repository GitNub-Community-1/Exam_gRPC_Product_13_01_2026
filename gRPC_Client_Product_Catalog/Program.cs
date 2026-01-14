using gRPC_Client;
using Grpc.Net.Client;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Добавляем контроллеры
builder.Services.AddControllers();

// Добавляем Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product Catalog gRPC Client API",
        Version = "v1"
    });
});

// Регистрируем gRPC‑клиент через DI
builder.Services.AddSingleton(new ProductService.ProductServiceClient(
    GrpcChannel.ForAddress("https://localhost:5001"))); // Убедись, что сервер слушает на этом порту

var app = builder.Build();

// Включаем Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();