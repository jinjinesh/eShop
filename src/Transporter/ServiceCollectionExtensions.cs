using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Transporter;

public  static class ServiceCollectionExtensions
{
    public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var rmqOption = new RabbitMqOption();
        configuration.GetSection("RabbitMQ").Bind(rmqOption);
        services.AddSingleton(rmqOption);
    }
}