using Grpc.Net.Client;
using gRPC_Client;

var builder = WebApplication.CreateBuilder(args);

// Add OpenAPI for convenience
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// A simple endpoint that creates a gRPC channel to the local server and calls GetAllProducts.
app.MapGet("/grpc-test", async () =>
{
    using var channel = GrpcChannel.ForAddress("https://localhost:5001");
    var client = new ProductService.ProductServiceClient(channel);
    var reply = await client.GetAllProductsAsync(new Get_All_Products_Request());
    return Results.Ok(new { count = reply.Products.Count });
});

app.Run();