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

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var logger = new Loggers();

            try
            {
                //Creating Scheduler
                Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
                Scheduler.JobFactory = _jobFactory;
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