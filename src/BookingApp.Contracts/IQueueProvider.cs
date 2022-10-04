namespace BookingApp.Contracts
{
    /// <summary>
    /// QueueProvider interface
    /// </summary>
    public interface IQueueProvider
    {
        /// <summary>
        /// SendMessage2Queue method
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> SendMessage2Queue(string message, CancellationToken cancellationToken = default);
    }
}
