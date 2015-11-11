using FlightBooker.Host;
using FlightBooker.Messages;
using NUnit.Framework;

namespace FlightBooker.Test
{
     [TestFixture]
    public class FlightBookerSagaTests
    {
        [Test]
        public void WhenFlightBookingTimesOut()
        {
            var customerId = 1;
            var startBookingCommand = new StartBookingCommand { CustomerId = customerId, TotalTicketAmount = 11000 };

            NServiceBus.Testing.Test.Saga<FlightBookerSaga>()
                .When(s => s.Handle(startBookingCommand))
                .WhenSagaTimesOut()
                .ExpectPublish<UpgradeCustomerToVIP>(d => d.CustomerId == customerId);
        }
    }
}
