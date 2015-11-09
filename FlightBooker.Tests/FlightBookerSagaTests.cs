using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightBooker.Host;
using FlightBooker.Messages;
using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;

namespace FlightBooker.Tests
{
    [TestFixture]
    public class FlightBookerSagaTests
    {
        [Test]
        public void WhenFlightBookingTimesOut()
        {
            var startBookingCommand = new StartBookingCommand { CustomerId = 1, TotalTicketAmount = 11000};

            Test.Saga<FlightBookerSaga>()
                .When(s => s.Handle(startBookingCommand))
                .ExpectPublish<DegradeCustomerToRegular>(d => d.CustomerId == 2);
        }
    }
}
