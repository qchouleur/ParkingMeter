using System;

namespace ParkingMeter.Logic.TimeRules
{
    public class FreeMonthRule
    {
        public int MonthOfTheYear { get; private set; }

        public FreeMonthRule(int monthOfTheYear)
        {
            MonthOfTheYear = monthOfTheYear;
        }

        public DateTime ApplyOn(DateTime date)
        {
            return new DateTime(date.Year, MonthOfTheYear + 1, 1);
        }
    }
}
