using NServiceBus.Saga;

namespace FlightBooker.Host
{
    public class FlightBookerSagaData : ContainSagaData
    {
        [Unique]
        public virtual int CustomerId { get; set; }

        public virtual double RunningTotal { get; set; }

        public virtual bool IsVip { get; set; } 
    }
}