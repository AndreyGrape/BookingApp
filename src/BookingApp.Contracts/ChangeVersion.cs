namespace BookingApp.Contracts
{
    /// <summary>
    /// Container last version values
    /// </summary>
    public sealed class ChangeVersion
    {
        public long LastClientVersion { get; set; }

        public long LastEventVersion { get; set; }

        public long LastBookingVersion { get; set; }
    }
}
