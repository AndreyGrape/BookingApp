namespace BookingApp.Contracts
{
    /// <summary>
    /// QueueBoxProvider interface
    /// </summary>
    public interface IQueueBoxProvider
    {
        /// <summary>
        /// GetQueueUrl
        /// </summary>
        /// <returns></returns>
        public string GetQueueUrl();
    }
}
