namespace BookingApp.DataAccessLayer.Contracts
{
    public sealed class Event
    {
        public int EventId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime EventDate { get; set; }


        public char ChOperation { get; set; }
    }
}
