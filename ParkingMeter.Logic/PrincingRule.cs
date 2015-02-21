namespace ParkingMeter.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PrincingRule
    {
        private readonly Dictionary<decimal, TimeSpan> _pricingRules = new Dictionary<decimal, TimeSpan>()
        {
            {1m, new TimeSpan(0, 20, 0)},
            {2m, new TimeSpan(1, 0, 0)},
            {3m, new TimeSpan(2, 0, 0)},
            {5m, new TimeSpan(12, 0, 0)},
            {8m, new TimeSpan(24, 0, 0)}
        };

        public TimeSpan CalculateTimeLimitForAmount(decimal amount)
        {
            var minimalAmount = _pricingRules.Keys.Min();

            if (amount < minimalAmount)
            {
                return new TimeSpan(0);
            }

            var remaining = amount;
            var totalTimeLimit = new TimeSpan(0);
            while (remaining >= minimalAmount)
            {
                var maximumPossibleAmount = _pricingRules.Keys.Where(price => remaining >= price).Max();
                totalTimeLimit += _pricingRules[maximumPossibleAmount];
                remaining -= maximumPossibleAmount;
            }

            return totalTimeLimit;
        }
    }
}
