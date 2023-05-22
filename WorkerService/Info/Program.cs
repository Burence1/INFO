using Info;
using Quartz.Impl;
using Quartz;
using Quartz.Spi;
using Info.Utils;
using Serilog;
using Info.Jobs;
using Info.Models;
using Info.Implementations;

var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .AddJsonFile("appsettings.json")
    .Build();

var host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((hostContext, services) =>
    {
        //Logger configuration
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(hostContext.Configuration).CreateLogger();

        services.AddSingleton<IJobFactory,JobFactory>();
        services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

        #region Register JobType To Services
        services.AddSingleton<ETLAutomationJob>();
        #endregion

        #region Executing Job

        var jobMetadata = new List<JobMetadata>
        {
            new(Guid.NewGuid(),typeof(ETLAutomationJob),"ETL Process Job",RetrieveCronExpressions.RunETLExpression(configuration).Result)
        };

        services.AddSingleton(jobMetadata);

        #endregion

        services.AddHostedService<Worker>();
    }).Build();

await host.RunAsync();
