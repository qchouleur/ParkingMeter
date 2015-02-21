using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingMeter.Logic
{
    public class ParkingMeter
    {
        private readonly decimal[] _validCoins = { 0.2m, 0.5m, 1m, 2m };
        
        public decimal TotalAmountOfMoney { get; private set; }
        private readonly IClock _clock;

        public ParkingMeter(IClock clock)
        {
            _clock = clock;
        }

        public void AddCoin(decimal coinValue)
        {
            if (!_validCoins.Contains(coinValue))
            {
                throw new ArgumentException("{0} is not a valid coin value");
            }

            TotalAmountOfMoney += coinValue;
        }

        public DateTime GetParkingLimitDate()
        {
            return _clock.Now + ComputeLimitTime();
        }

        private readonly Dictionary<decimal, TimeSpan> _pricingRules = new Dictionary<decimal, TimeSpan>()
        {
            {1m, new TimeSpan(0, 20, 0)},
            {2m, new TimeSpan(1, 0, 0)},
            {3m, new TimeSpan(2, 0, 0)},
            {5m, new TimeSpan(12, 0, 0)},
            {8m, new TimeSpan(24, 0, 0)}
        };

        private TimeSpan ComputeLimitTime()
        {
            var minimalAmount = _pricingRules.Keys.Min();
            
            if (TotalAmountOfMoney < minimalAmount)
            {
                return new TimeSpan(0);
            }

            var remaining = TotalAmountOfMoney;
            var totalTimeLimit = new TimeSpan(0);
            while (remaining >= minimalAmount)
            {
                foreach (var possibleAmount in _pricingRules.Keys.OrderByDescending(price => price))
                {
                    if (remaining >= possibleAmount)
                    {
                        totalTimeLimit = totalTimeLimit + _pricingRules[possibleAmount];
                        remaining -= possibleAmount;
                        break;
                    }
                }
            }

            return totalTimeLimit;
        }

    }
}
