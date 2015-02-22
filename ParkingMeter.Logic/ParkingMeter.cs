using ParkingMeter.Logic.TimeRules;

namespace ParkingMeter.Logic
{
    using System;
    using System.Linq;

    public class ParkingMeter
    {
        private readonly decimal[] _validCoins = { 0.2m, 0.5m, 1m, 2m };
        
        public decimal TotalAmountOfMoney { get; private set; }

        private readonly PrincingRule _pricingRule;
        private readonly TimeLimitRule _timeLimitRule;

        public ParkingMeter(IClock clock, TimeRulesConfiguration configuration)
        {
            _timeLimitRule = new TimeLimitRule(clock, configuration);
            _pricingRule = new PrincingRule();
        }

        public void AddCoin(decimal coinValue)
        {
            if (!_validCoins.Contains(coinValue))
            {
                throw new ArgumentException("{0} is not a valid coin value");
            }

            TotalAmountOfMoney += coinValue;
        }

        public double GetCurrentTimeLimitInHour()
        {
            return _pricingRule.CalculateTimeLimitForAmount(TotalAmountOfMoney).TotalHours;
        }

        public DateTime GetParkingLimitDate()
        {
            var availableTime = _pricingRule.CalculateTimeLimitForAmount(TotalAmountOfMoney);

            return _timeLimitRule.ComputeEndingTimeDate(availableTime);
        }
    }
}
