namespace FlightBooker.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string From { get; set; }

        public string To { get; set; }

        public double Price { get; set; }

        public int NumberOfTickets { get; set; }

    }
}