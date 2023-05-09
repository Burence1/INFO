using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Spi;

namespace Info.Utils
{
    internal class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _service;

        public JobFactory(IServiceProvider serviceProvider)
        {
            _service = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;
            return (IJob)_service.GetService(jobDetail.JobType);
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}
