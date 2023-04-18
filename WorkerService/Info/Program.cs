using Info;
using Quartz.Impl;
using Quartz;
using Quartz.Spi;

var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .AddJsonFile("appsettings.json")
    .Build();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        
        services.AddSingleton<IJobFactory,IJobFactory>();
        services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
