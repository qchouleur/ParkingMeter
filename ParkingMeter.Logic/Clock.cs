namespace ParkingMeter.Logic
{
    using System;

    public class Clock : IClock
    {
        public DateTime Now
        {
            get { return DateTime.Now; }
        }
    }
}
