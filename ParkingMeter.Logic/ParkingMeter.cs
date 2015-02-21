namespace ParkingMeter.Logic
{
    using System;
    using System.Linq;

    public class ParkingMeter
    {
        private readonly decimal[] _validCoins = { 0.2m, 0.5m, 1m, 2m };
        
        public decimal TotalAmountOfMoney { get; private set; }

        private readonly PrincingRule _pricingRule;
        private readonly IClock _clock;

        public ParkingMeter(IClock clock)
        {
            _clock = clock;
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

        public DateTime GetParkingLimitDate()
        {
            return _clock.Now + _pricingRule.CalculateTimeLimitForAmount(TotalAmountOfMoney);
        }
    }
}
