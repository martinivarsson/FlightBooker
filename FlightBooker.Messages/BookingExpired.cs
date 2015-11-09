using NServiceBus;

namespace FlightBooker.Messages
{
    public class BookingExpired : IMessage
    {
        public double TotalAmount { get; set; }
    }
}