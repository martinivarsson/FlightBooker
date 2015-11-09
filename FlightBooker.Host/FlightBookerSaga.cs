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
            this.Data.CustomerId = message.CustomerId;
            this.Data.RunningTotal += message.TotalTicketAmount;

            this.RequestTimeout<BookingExpired>(TimeSpan.FromSeconds(30),
        timeout => timeout.TotalAmount = message.TotalTicketAmount);

            CheckForPreferredStatusChange();
        }

        public void Timeout(BookingExpired state)
        {
            this.Data.RunningTotal -= state.TotalAmount;

            CheckForPreferredStatusChange();
        }

        private void CheckForPreferredStatusChange()
        {
            if (this.Data.IsVip == false && this.Data.RunningTotal >= 10000)
            {
                Data.IsVip = true;
                Console.WriteLine("BECOMING VIP");
                Bus.Publish(new UpgradeCustomerToVIP {CustomerId = Data.CustomerId});
            }
            else if (this.Data.IsVip == true && this.Data.RunningTotal < 10000)
            {
                Data.IsVip = false;
                Console.WriteLine("BECOMING REGULAR");
                Bus.Publish(new DegradeCustomerToRegular{ CustomerId = Data.CustomerId });
            }
        }
    }
}