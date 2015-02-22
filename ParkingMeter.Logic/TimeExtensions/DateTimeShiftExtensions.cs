namespace ParkingMeter.Logic.TimeExtensions
{
    using System;

    public static class DateTimeShiftExtensions
    {
        public static DateTime ShiftToNextDay(this DateTime date)
        {
            var nextDay = date.AddDays(1);
            return new DateTime(nextDay.Year, nextDay.Month, nextDay.Day);
        }

        public static DateTime ShiftToHour(this DateTime date, int hour)
        {
            return new DateTime(date.Year, date.Month, date.Day, hour, 0, 0);
        }

        public static DateTime ShiftToMonth(this DateTime date, int month)
        {
            var shiftSize = date.Month > month ? month + 12 - date.Month : date.Month - month;
            var nextMonth = date.AddMonths(shiftSize);
            return new DateTime(nextMonth.Year, month, 1);
        }
    }
}
