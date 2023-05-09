using Info;
using Quartz.Impl;
using Quartz;
using Quartz.Spi;
using Info.Utils;
using Serilog;

var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .AddJsonFile("appsettings.json")
    .Build();

IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .UseWindowsService()
    .ConfigureServices((hostContext, services) =>
    {
        //Logger configuration
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(hostContext.Configuration).CreateLogger();

        services.AddSingleton<IJobFactory,JobFactory>();
        services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

        services.AddHostedService<Worker>();
    }).Build();

await host.RunAsync();
