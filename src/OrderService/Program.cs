using OrderService.Services;
using Transporter;

var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";
var builder = WebApplication.CreateBuilder(args);
var configuration = GetConfiguration(environment);
builder.Services.AddRabbitMq(configuration);
// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<OrderService.Services.OrderService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

IConfiguration GetConfiguration(string environment)
{
    return new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{environment}.json", true, true)
        .AddJsonFile($"secrets/appsettings.json", true, true)
        .AddEnvironmentVariables()
        .Build();
}
