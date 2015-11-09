using NServiceBus;

namespace FlightBooker.Messages
{
    public class DegradeCustomerToRegular : IEvent
    {
        public int CustomerId { get; set; }
    }
}