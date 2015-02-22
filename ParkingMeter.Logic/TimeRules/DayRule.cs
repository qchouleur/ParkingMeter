using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingMeter.Logic.TimeRules
{
    public class DayRule
    {
        public DayOfWeek DayOfWeek { get; private set; }
        public List<TimePeriodRule> TimePeriodRules { get; private set; }

        public DayRule(DayOfWeek dayOfWeek, IEnumerable<TimePeriodRule> timePeriodRules)
        {
            DayOfWeek = dayOfWeek;
            TimePeriodRules = new List<TimePeriodRule>();

            foreach (var timePeriodRule in timePeriodRules)
            {
                Add(timePeriodRule);
            }
        }

        private void Add(TimePeriodRule timePeriodRule)
        {
            if (!TimePeriodRules.Any(period => period.OverlapsWith(timePeriodRule)))
            {
                TimePeriodRules.Add(timePeriodRule);
                return;
            }
            while (TimePeriodRules.Any(period => period.OverlapsWith(timePeriodRule) 
                && !period.Equals(timePeriodRule)))
            {
                var overlappingPeriod = TimePeriodRules.First(period => period.OverlapsWith(timePeriodRule));
                timePeriodRule = timePeriodRule.MergeWith(overlappingPeriod);
                TimePeriodRules.Remove(overlappingPeriod);
            }
            TimePeriodRules.Add(timePeriodRule);
        }
    }
}
