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
                    /*//Create Job
                    var jobDetail = CreateJob(jobMetadata);

                    //Create trigger
                    var trigger = CreateTrigger(jobMetadata);

                    //Schedule Job
                    Scheduler.ScheduleJob(jobDetail, trigger, cancellationToken).GetAwaiter();*/
                });
                //Start The Scheduler
                await Scheduler.Start(cancellationToken);
            }
            catch (Exception ex)
            {

            }
            throw new NotImplementedException();
        }



        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}