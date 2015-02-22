using System;
using System.Linq;
using ParkingMeter.Logic.TimeExtensions;
using ParkingMeter.Logic.TimeRules;

namespace ParkingMeter.Logic
{
    public class TimeLimitRule
    {
        private readonly ParkingRestrictions _parkingRestrictions;
        private readonly IClock _clock;

        public TimeLimitRule(IClock clock, TimeRulesConfiguration configuration)
        {
            if (!configuration.DayRules.Any(rule => rule.TimePeriodRules.Any()))
            {
                throw new ArgumentException("The time rules configuration should at least contain one time period rule");
            }

            if (clock == null)
            {
                throw new ArgumentNullException("clock");
            }

            _clock = clock;
            _parkingRestrictions = new ParkingRestrictions(configuration);

        }

        public DateTime ComputeEndingTimeDate(TimeSpan timeToSpend)
        {
            var currentEndingDate = _clock.Now;
            do
            {
                currentEndingDate = ShiftDateToNextTimeRestriction(currentEndingDate);
                var timeRestriction = GetNextTimeRestriction(currentEndingDate);
                if (timeToSpend == TimeSpan.Zero) return currentEndingDate;

                var timeToReachEndOfPeriod = timeRestriction.TimeToReachPeriodEnd(currentEndingDate.TimeOfDay);
                if (timeToSpend >= timeToReachEndOfPeriod)
                {
                    currentEndingDate = currentEndingDate.ShiftToHour(timeRestriction.EndHour);
                    timeToSpend = timeToSpend - timeToReachEndOfPeriod;
                }
                else
                {
                    currentEndingDate = currentEndingDate + timeToSpend;
                    timeToSpend = TimeSpan.Zero;
                }
            } while (timeToSpend > TimeSpan.Zero);

            return currentEndingDate;
        }

        private TimePeriodRule GetNextTimeRestriction(DateTime date)
        {
            return _parkingRestrictions.GetNextMatchingTimePeriodRule(date);
        }

        private DateTime ShiftDateToNextTimeRestriction(DateTime date)
        {
            if (_parkingRestrictions.IsInRestrictedTimePeriod(date))
            {
                return date;
            }

            if (_parkingRestrictions.IsInFreeMonth(date))
            {
                var monthRule = _parkingRestrictions.MatchingFreeMonthRuleForDate(date);
                date = monthRule.ApplyOn(date);
            }

            if (_parkingRestrictions.IsInFreeDay(date))
            {
                date = new DateTime(date.Year, date.Month, date.Day + 1);
            }

            if (_parkingRestrictions.IsInFreeTimePeriod(date))
            {
                date = _parkingRestrictions.GetNextTimeRestrictionStartDate(date);
            }

            return date;
        }
    }
}
