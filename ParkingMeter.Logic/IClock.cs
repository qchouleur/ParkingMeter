namespace ParkingMeter.Logic
{
    using System;

    public interface IClock
    {
        DateTime Now { get; }
    }
}
