namespace BookingApp.Infrastructure.Quartz
{
    public class JobParamsWrapper
    {
        public JobParamsWrapper(Type jobType, string cronExpression)
        {
            JobType = jobType;
            CronExpression = cronExpression;
        }

        public Type JobType { get; }
        public string CronExpression { get; }
    }
}
