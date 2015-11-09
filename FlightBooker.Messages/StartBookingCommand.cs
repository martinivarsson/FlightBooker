using NServiceBus;

namespace FlightBooker.Messages
{
    public class StartBookingCommand : ICommand
    {
        public int CustomerId { get; set; } 
        public double TotalTicketAmount { get; set; }
    }
}