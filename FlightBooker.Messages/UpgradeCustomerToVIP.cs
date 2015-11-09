using NServiceBus;

namespace FlightBooker.Messages
{
    public class UpgradeCustomerToVIP : IEvent
    {
        public int CustomerId { get; set; }
    }
}