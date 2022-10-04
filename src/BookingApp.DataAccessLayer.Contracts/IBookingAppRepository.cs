namespace BookingApp.DataAccessLayer.Contracts
{
    /// <summary>
    /// Data Access Layer
    /// </summary>
    public interface IBookingAppRepository
    {
        Task<IEnumerable<Client>> GetClientsChange(CancellationToken cancellationToken = default);

        Task<IEnumerable<Booking>> GetBookingsChange(CancellationToken cancellationToken = default);

        Task<IEnumerable<Event>> GetEventsChange(CancellationToken cancellationToken = default);
    }
}
