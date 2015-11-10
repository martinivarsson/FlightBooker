using System;
using FlightBooker.Messages;
using NServiceBus.Saga;

namespace FlightBooker.Host
{
    public class FlightBookerSaga : Saga<FlightBookerSagaData>,
        IAmStartedByMessages<StartBookingCommand>,
        IHandleTimeouts<BookingExpired>
    {
        protected override void ConfigureHowToFindSaga
            (SagaPropertyMapper<FlightBookerSagaData> mapper)
        {
            mapper.ConfigureMapping<StartBookingCommand>(msg => msg.CustomerId)
                .ToSaga(s => s.CustomerId);
        }
        public void Handle(StartBookingCommand message)
        {
            Data.CustomerId = message.CustomerId;
            Data.RunningTotal += message.TotalTicketAmount;

            RequestTimeout<BookingExpired>(TimeSpan.FromSeconds(60),
            timeout => timeout.TotalAmount = message.TotalTicketAmount);

            CheckForPreferredStatusChange();
        }

        public void Timeout(BookingExpired state)
        {
            Data.RunningTotal -= state.TotalAmount;

            CheckForPreferredStatusChange();
        }

        private void CheckForPreferredStatusChange()
        {
            if (Data.IsVip == false && Data.RunningTotal >= 10000)
            {
                Data.IsVip = true;
                Console.WriteLine("BECOMING VIP");
                Bus.Publish(new UpgradeCustomerToVIP {CustomerId = Data.CustomerId});
            }
            else if (Data.IsVip && Data.RunningTotal < 10000)
            {
                Data.IsVip = false;
                Console.WriteLine("BECOMING REGULAR");
                Bus.Publish(new DegradeCustomerToRegular{ CustomerId = Data.CustomerId });
            }
        }
    }
}