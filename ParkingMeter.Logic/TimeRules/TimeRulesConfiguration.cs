using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingMeter.Logic.TimeRules
{
    public class TimeRulesConfiguration
    {
        private readonly List<DayRule> _dayRules = new List<DayRule>();

        public IEnumerable<DayRule> DayRules
        {
            get { return _dayRules; }
        }

        private readonly List<FreeMonthRule> _freeMonthRules = new List<FreeMonthRule>();

        public IEnumerable<FreeMonthRule> FreeMonthRules
        {
            get { return _freeMonthRules; }
        }

        public void Add(DayRule dayRule)
        {
            if (_dayRules.Any(rule => rule.DayOfWeek == dayRule.DayOfWeek))
            {
                throw new ArgumentException(string.Format("A rule already exist for day of the week {0}",
                    dayRule.DayOfWeek));
            }

            _dayRules.Add(dayRule);
        }

        public void Add(FreeMonthRule freeMonthRule)
        {
            if (_freeMonthRules.Any(rule => rule.MonthOfTheYear == freeMonthRule.MonthOfTheYear))
            {
                throw new ArgumentException(string.Format("A rule already exist for month of the year {0}",
                    freeMonthRule.MonthOfTheYear));
            }

            _freeMonthRules.Add(freeMonthRule);
        }
    }
}
