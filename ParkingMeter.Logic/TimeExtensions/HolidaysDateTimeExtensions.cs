using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingMeter.Logic.TimeExtensions
{
    public static class HolidaysDateTimeExtensions
    {
        private static readonly List<Func<int, DateTime>> FixedFrenchHolidays =
            new List<Func<int, DateTime>>
            {
                year => new DateTime(year, 1, 1), // New year
                year => new DateTime(year, 5, 1), // Labour day
                year => new DateTime(year, 5, 8), // Victory in europe day
                year => new DateTime(year, 7, 14), // Bastille day
                year => new DateTime(year, 8, 15), // Assumption
                year => new DateTime(year, 11, 1), // All saints' day
                year => new DateTime(year, 11, 11), // Armistice 
                year => new DateTime(year, 12, 25), // Christmas
            };

        private static IEnumerable<DateTime> VariableFrenchHolidaysInYear(this DateTime dateTime)
        {
            var easter = GetEasterDate(dateTime.Year);

            yield return easter.AddDays(1);  // Easter's monday
            yield return easter.AddDays(39); // Feast of the ascension
            yield return easter.AddDays(50); // Pentecost
        }

        public static DateTime GetEasterDate(int year)
        {
            // the computing method was found 
            // at : aa.usno.navy.mil/faq/docs/easter.php

            var century = year / 100;
            var n = year - 19 * (year / 19);
            var k = (century - 17) / 25;
            var i = century - century / 4 - (century - k) / 3 + 19 * n + 15;
            i = i - 30 * (i / 30);
            i = i - (i / 28) * (1 - (i / 28) * (29 / (i + 1)) * ((21 - n) / 11));
            var j = year + year / 4 + i + 2 - century + century / 4;
            j = j - 7 * (j / 7);
            var l = i - j;
            var month = 3 + (l + 40) / 44;
            var day = l + 28 - 31 * (month / 4);

            return new DateTime(year, month, day);
        }

        public static IEnumerable<DateTime> FrenchHolidaysInYear(this DateTime dateTime)
        {
            return
                dateTime.VariableFrenchHolidaysInYear()
                .Union(FixedFrenchHolidays
                        .Select(h => h.Invoke(dateTime.Year)));
        }
    }
}
