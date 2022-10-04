namespace BookingApp.Contracts
{
    /// <summary>
    /// Business Logic Layer
    /// </summary>
    public interface IApplicationProcessing
    {
        /// <summary>
        /// Looking for change in data.
        /// </summary>
        /// <param name="entityType">Kind of Entity</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns></returns>
        Task<string> LookingForChange(EntityType entityType, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send message to queue.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns></returns>
        Task<bool> SendToQueue(string message, CancellationToken cancellationToken = default);

        /// <summary>
        /// Safe key\value in store.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns></returns>
        Task Safe2KeyValueStore(string key, string message, CancellationToken cancellationToken = default);
    }
}