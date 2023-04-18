using Info.Models;
using Info.Utils;
using Quartz;
using Quartz.Spi;

namespace Info
{
    public class Worker : IHostedService
    {
        private readonly ILogger<Worker> _logger;

        public IScheduler? Scheduler { get; set; }
        private readonly IJobFactory _jobFactory;
        private readonly List<JobMetadata> _jobMetadatas;
        private readonly ISchedulerFactory _schedulerFactory;

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
                .WithDescription(jobMetadata.JobName).Build();
        }

        private static IJobDetail CreateJob(JobMetadata jobMetadata)
        {
            return JobBuilder.Create()
                .WithIdentity (jobMetadata.JobId.ToString())
                .WithDescription (jobMetadata.JobName).Build();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
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
            catch (Exception ex)
            {

            }
        }



        public Task StopAsync(CancellationToken cancellationToken)
        {
        }
    }
}