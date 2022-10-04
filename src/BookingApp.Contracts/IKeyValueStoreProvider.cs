namespace BookingApp.Contracts
{
    /// <summary>
    /// KeyValueStoreProvider interface
    /// </summary>
    public interface IKeyValueStoreProvider
    {
        /// <summary>
        /// SetKeyValueAsync method
        /// </summary>
        /// <param name="key"></param>
        /// <param name="consulValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SetKeyValueAsync(string key, string consulValue, CancellationToken cancellationToken = default);
    }
}
