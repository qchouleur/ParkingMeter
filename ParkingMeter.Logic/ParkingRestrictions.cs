using System;
using System.Collections.Generic;
using System.Linq;
using ParkingMeter.Logic.TimeExtensions;
using ParkingMeter.Logic.TimeRules;

namespace ParkingMeter.Logic
{
    public class ParkingRestrictions
    {
        private readonly TimeRulesConfiguration _timeRulesConfiguration;

        private IEnumerable<DayRule> DayRules 
        {
            get { return _timeRulesConfiguration.DayRules; }   
        }

        private IEnumerable<FreeMonthRule> FreeMonthRules 
        {
            get { return _timeRulesConfiguration.FreeMonthRules; }   
        }

        public ParkingRestrictions(TimeRulesConfiguration configuration)
        {
            _timeRulesConfiguration = configuration;
        }

        public bool IsInFreeMonth(DateTime date)
        {
            return FreeMonthRules.Any(m => m.MonthOfTheYear == date.Month);
        }

        public bool IsInFreeDay(DateTime date)
        {
            return date.FrenchHolidaysInYear().Any(d => d == date) 
                || DayRules.All(rule => rule.DayOfWeek != date.DayOfWeek);
        }

        public bool IsInFreeTimePeriod(DateTime date)
        {
            var dayRule = DayRules.FirstOrDefault(rule => rule.DayOfWeek == date.DayOfWeek);
            return dayRule == null || !dayRule.TimePeriodRules.Any(period => period.Contains(date.TimeOfDay));
        }

        public bool IsInRestrictedTimePeriod(DateTime date)
        {
            var matchingDayRule = DayRules.FirstOrDefault(rule => rule.DayOfWeek == date.DayOfWeek);

            return matchingDayRule != null && matchingDayRule.TimePeriodRules.Any(
                period => period.Contains(date.TimeOfDay));
        }

        public FreeMonthRule MatchingFreeMonthRuleForDate(DateTime date)
        {
            return FreeMonthRules.First(month => month.MonthOfTheYear == date.Month);
        }

        public TimePeriodRule GetNextMatchingTimePeriodRule(DateTime date)
        {
            var nextRestrictionStartDate = date;

            if (IsInRestrictedTimePeriod(date))
            {
                return
                    DayRules.First(rule => rule.DayOfWeek == date.DayOfWeek)
                        .TimePeriodRules.First(period => period.Contains(date.TimeOfDay));
            }

            while (true)
            {
                if (DayRules.Any(rule => rule.DayOfWeek == nextRestrictionStartDate.DayOfWeek))
                {
                    var dayRule = DayRules.First(rule => rule.DayOfWeek == nextRestrictionStartDate.DayOfWeek);

                    var nextTimePeriod =
                        dayRule.TimePeriodRules.FirstOrDefault(period => 
                            period.StartHour >= nextRestrictionStartDate.TimeOfDay.Hours);

                    if (nextTimePeriod != null) 
                    {
                        return nextTimePeriod;
                    }
                }

                nextRestrictionStartDate = nextRestrictionStartDate.ShiftToNextDay(); 
            }
        }

        public DateTime GetNextTimeRestrictionStartDate(DateTime date)
        {
            var nextRestrictionStartDate = date;
            while (true)
            {
                if (DayRules.Any(rule => rule.DayOfWeek == nextRestrictionStartDate.DayOfWeek))
                {
                    var dayRule = DayRules.First(rule => rule.DayOfWeek == nextRestrictionStartDate.DayOfWeek);
                    var nextTimePeriod =
                        dayRule.TimePeriodRules.FirstOrDefault(
                            period => period.StartHour >= nextRestrictionStartDate.TimeOfDay.Hours);

                    if (nextTimePeriod != null)
                    {
                        return nextRestrictionStartDate.ShiftToHour(nextTimePeriod.StartHour);
                    }
                }

                nextRestrictionStartDate = nextRestrictionStartDate.ShiftToNextDay();
            }
        }
    }
}
