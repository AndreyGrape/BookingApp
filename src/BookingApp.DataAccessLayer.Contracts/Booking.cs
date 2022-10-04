namespace BookingApp.DataAccessLayer.Contracts
{
    public sealed class Booking
    {
        public int BookingId { get; set; }

        public int ClientId { get; set; }

        public int EventId { get; set; }

        public string SeatNumber { get; set; }


        public char ChOperation { get; set; }
    }
}
