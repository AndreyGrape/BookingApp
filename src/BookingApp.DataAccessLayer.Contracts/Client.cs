namespace BookingApp.DataAccessLayer.Contracts
{
    public sealed class Client
    {
        public int ClientId { get; set; }

        public string FirstName { get; set; }

        public string SurName { get; set; }

        public DateTime DateOfBirth { get; set; }


        public char ChOperation { get; set; } 
    }
}
