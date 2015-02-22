namespace ParkingMeter.Logic.Tests
{
    using System;

    public class FakeClock : IClock
    {
        private readonly DateTime _now;

        public FakeClock(DateTime now)
        {
            _now = now;
        }

        public DateTime Now
        {
            get { return _now; }
        }

        public DateTime Today
        {
            get { return new DateTime(_now.Year, _now.Month, _now.Day); }
        }
    }
}
