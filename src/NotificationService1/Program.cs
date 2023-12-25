using NotificationService1;
using System;
using Transporter;


var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";
var configuration = GetConfiguration(environment);
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddRabbitMq(configuration);
builder.Services.AddHostedService<CreationEventHandler>();

var host = builder.Build();
host.Run();

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