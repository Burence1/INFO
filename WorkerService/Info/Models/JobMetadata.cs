namespace Info.Models
{
    public class JobMetadata
    {
        public Guid JobId { get; set; }
        public Type JobType { get; set; }
        public string JobName { get; set; }
        public string CronExpression { get; set; }


        public JobMetadata(Guid jobId,Type jobType,string jobName,string cronExpression) { 
            JobId = jobId;
            JobType = jobType;
            JobName = jobName;
            CronExpression = cronExpression;
        }
    }
}