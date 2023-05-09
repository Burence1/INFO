using Info.Models;
using Info.Utils;
using Quartz;
using Quartz.Spi;
using System.Reflection;
using System.Threading;

namespace Info
{
    internal class Worker : BackgroundService

    {

        private readonly ILogger<Worker> _logger;

        public IScheduler? Scheduler { get; set; }
        private readonly IJobFactory _jobFactory;
        private readonly List<JobMetadata> _jobMetadatas;
        private readonly ISchedulerFactory _schedulerFactory;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var logger = new Loggers();
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Worker Started at: {time}", DateTimeOffset.Now);

                }
            }
            catch (Exception ex)
            {
                var methodName = MethodBase.GetCurrentMethod().ReflectedType.Name;
                logger.LogMethodsErrorDetails(methodName, ex, 1, 4);
            }

        }

        public async override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker Started at: {time}", DateTimeOffset.Now);
            var logger = new Loggers();
            try
            {
                //Creating Scheduler
                Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
                Scheduler.JobFactory = _jobFactory;

                //Support for Multiple Jobs
                _jobMetadatas.ForEach(jobMetadata =>
                {
                    //Create Job
                    var jobDetail = CreateJob(jobMetadata);

                    //Create trigger
                    var trigger = CreateTrigger(jobMetadata);

                    //Schedule Job
                    Scheduler.ScheduleJob(jobDetail, trigger, cancellationToken).GetAwaiter();
                });
                //Start The Scheduler
                await Scheduler.Start(cancellationToken);
            }
            catch (Exception e)
            {
                var methodName = MethodBase.GetCurrentMethod().ReflectedType.Name;
                logger.LogMethodsErrorDetails(methodName, e, 1, 4);
            }
            
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker Stopped at: {time}", DateTimeOffset.Now);

            return base.StartAsync(cancellationToken);
        }


        public Worker(ISchedulerFactory schedulerFactory, List<JobMetadata> jobMetadatas, IJobFactory jobFactory)
        {
            this._jobFactory = jobFactory;
            this._schedulerFactory = schedulerFactory;
            this._jobMetadatas = jobMetadatas;
        }

        private static ITrigger CreateTrigger(JobMetadata jobMetadata)
        {
            return TriggerBuilder.Create()
                .WithIdentity(jobMetadata.JobId.ToString())
                .WithCronSchedule(jobMetadata.CronExpression)
                .WithDescription(jobMetadata.JobName)
                .Build();
        }

        private static IJobDetail CreateJob(JobMetadata jobMetadata)
        {
            return JobBuilder.Create()
                .WithIdentity(jobMetadata.JobId.ToString())
                .WithDescription(jobMetadata.JobName).Build();
        }

        //public async Task StartAsync(CancellationToken cancellationToken)
        //{
        //    var logger = new Loggers();

        //    try
        //    {
        //        //Creating Scheduler
        //        Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
        //        Scheduler.JobFactory = _jobFactory;

        //        //Support for Multiple Jobs
        //        _jobMetadatas.ForEach(jobMetadata =>
        //        {
        //            //Create Job
        //            var jobDetail = CreateJob(jobMetadata);

        //            //Create trigger
        //            var trigger = CreateTrigger(jobMetadata);

        //            //Schedule Job
        //            Scheduler.ScheduleJob(jobDetail, trigger, cancellationToken).GetAwaiter();
        //        });
        //        //Start The Scheduler
        //        await Scheduler.Start(cancellationToken);
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = MethodBase.GetCurrentMethod().ReflectedType.Name;
        //        logger.LogMethodsErrorDetails(methodName, ex, 1, 4);
        //    }
        //}

        //public async Task StopAsync(CancellationToken cancellationToken)
        //{
        //    await Scheduler.Shutdown(cancellationToken).ConfigureAwait(false);
        //}

        //protected override Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    throw new NotImplementedException();
        //}
    }
}