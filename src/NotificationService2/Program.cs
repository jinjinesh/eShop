using NotificationService2;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<CreationEventHandler>();
builder.Services.AddHostedService<UpdationEventHandler>();

var host = builder.Build();
host.Run();
