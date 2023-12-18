using NotificationService1;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<CreationEventHandler>();

var host = builder.Build();
host.Run();
