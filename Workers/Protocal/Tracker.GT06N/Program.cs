using Tracker.GT06N;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<GT06NWorker>();
    })
    .Build();

host.Run();
