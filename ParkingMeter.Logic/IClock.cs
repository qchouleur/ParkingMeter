namespace ParkingMeter.Logic
{
    using System;

    public interface IClock
    {
        DateTime Now { get; }
        DateTime Today { get; }
    }
}
