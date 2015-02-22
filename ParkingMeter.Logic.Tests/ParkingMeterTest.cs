using ParkingMeter.Logic.TimeRules;

namespace ParkingMeter.Logic.Tests
{
    using System;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class ParkingMeterTest
    {
        private ParkingMeter _parkingMeter;

        private TimeRulesConfiguration _timeRulesConfiguration;

        private FakeClock _clock;

        [SetUp]
        public void SetUp()
        {
            _clock = new FakeClock(DateTime.Now);
            
            _timeRulesConfiguration = new TimeRulesConfiguration();
            _timeRulesConfiguration.Add(new DayRule(DayOfWeek.Monday,
                new[] {new TimePeriodRule(8, 12), new TimePeriodRule(14, 18)}));
            _timeRulesConfiguration.Add(new DayRule(DayOfWeek.Tuesday,
                new[] {new TimePeriodRule(8, 12), new TimePeriodRule(14, 18)}));
            _timeRulesConfiguration.Add(new DayRule(DayOfWeek.Wednesday,
                new[] {new TimePeriodRule(8, 12), new TimePeriodRule(14, 18)}));
            _timeRulesConfiguration.Add(new DayRule(DayOfWeek.Thursday,
                new[] {new TimePeriodRule(8, 12), new TimePeriodRule(14, 18)}));
            _timeRulesConfiguration.Add(new DayRule(DayOfWeek.Friday,
                new[] {new TimePeriodRule(8, 12), new TimePeriodRule(14, 18)}));
            _timeRulesConfiguration.Add(new DayRule(DayOfWeek.Saturday,
                new[] {new TimePeriodRule(8, 12), new TimePeriodRule(14, 18)}));

            _timeRulesConfiguration.Add(new FreeMonthRule(8));

            _parkingMeter = new ParkingMeter(_clock, _timeRulesConfiguration);
        }

        #region Valid coin 

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ParkingMeter_AddCoin_WithInvalidCoinValue_ShouldFail()
        {
            // Arrange
            const decimal invalidCoinValue = 3m;

            // Act
            _parkingMeter.AddCoin(invalidCoinValue);
        }

        [Test]
        public void ParkingMeter_AddCoin_ShouldAddCoinValueToCurrentValue()
        {
            // Arrange
            decimal[] coins = { 0.2m, 0.2m, 0.5m, 1m };

            // Act
            foreach (var coin in coins)
            {
                _parkingMeter.AddCoin(coin);
            }

            // Assert
            Assert.That(_parkingMeter.TotalAmountOfMoney, Is.EqualTo(coins.Sum()));
        }

        #endregion

        #region Pricing Rule

        [Test]
        public void ParkingMeter_GetCurrentTimeLimitInHour_WithNoCoin_ShouldBeCurrentDate()
        {
            // Act
            var timeLimit = _parkingMeter.GetCurrentTimeLimitInHour();

            // Assert
            Assert.That(timeLimit, Is.EqualTo(0d));
        }

        [Test]
        public void ParkingMeter_GetCurrentTimeLimitInHour_WithLessThanOne_ShouldBeCurrentDate()
        {
            // Arrange
            _parkingMeter.AddCoin(0.2m);
            _parkingMeter.AddCoin(0.5m);

            // Act
            var timeLimit = _parkingMeter.GetCurrentTimeLimitInHour();

            // Assert
            Assert.That(timeLimit, Is.EqualTo(0d));
        }

        [Test]
        public void ParkingMeter_GetCurrentTimeLimitInHour_WithOne_ShouldBeIn20Minutes()
        {
            // Arrange
            const int timeLimitForOneInMinutes = 20;
            _parkingMeter.AddCoin(1m);

            // Act
            var timeLimit = _parkingMeter.GetCurrentTimeLimitInHour();

            // Assert
            Assert.That(timeLimit, Is.EqualTo(timeLimitForOneInMinutes / 60d));
        }

        [Test]
        public void ParkingMeter_GetCurrentTimeLimitInHour_WithTwo_ShouldBeIn1Hour()
        {
            // Arrange
            const int timeLimitForTwoInHour = 1;
            _parkingMeter.AddCoin(2m);

            // Act
            var timeLimit = _parkingMeter.GetCurrentTimeLimitInHour();

            // Assert
            Assert.That(timeLimit, Is.EqualTo(timeLimitForTwoInHour));
        }

        [Test]
        public void ParkingMeter_GetCurrentTimeLimitInHour_With3_ShouldBeIn2Hours()
        {
            // Arrange
            const int timeLimitFor3InHour = 2;
            _parkingMeter.AddCoin(2m);
            _parkingMeter.AddCoin(1m);

            // Act
            var timeLimit = _parkingMeter.GetCurrentTimeLimitInHour();

            // Assert
            Assert.That(timeLimit, Is.EqualTo(timeLimitFor3InHour));
        }

        [Test]
        public void ParkingMeter_GetCurrentTimeLimitInHour_With5_ShouldBeIn12Hours()
        {
            // Arrange
            const int timeLimitFor5InHour = 12;
            _parkingMeter.AddCoin(2m);
            _parkingMeter.AddCoin(2m);
            _parkingMeter.AddCoin(0.5m);
            _parkingMeter.AddCoin(0.5m);

            // Act
            var timeLimit = _parkingMeter.GetCurrentTimeLimitInHour();

            // Assert
            Assert.That(timeLimit, Is.EqualTo(timeLimitFor5InHour));
        }

        [Test]
        public void ParkingMeter_GetCurrentTimeLimitInHour_With8_ShouldBeIn24Hours()
        {
            // Arrange
            const int timeLimitFor8InHour = 24;
            _parkingMeter.AddCoin(2m);
            _parkingMeter.AddCoin(2m);
            _parkingMeter.AddCoin(2m);
            _parkingMeter.AddCoin(2m);

            // Act
            var timeLimit = _parkingMeter.GetCurrentTimeLimitInHour();

            // Assert
            Assert.That(timeLimit, Is.EqualTo(timeLimitFor8InHour));
        }

        [Test]
        public void ParkingMeter_GetCurrentTimeLimitInHour_With10_ShouldBeIn25Hours()
        {
            // Arrange
            const int timeLimitFor10InHour = 25;

            for (int i = 0; i < 5; i++)
            {
                _parkingMeter.AddCoin(2m);
            }

            // Act
            var timeLimit = _parkingMeter.GetCurrentTimeLimitInHour();

            // Assert
            Assert.That(timeLimit, Is.EqualTo(timeLimitFor10InHour));
        }

        [Test]
        public void ParkingMeter_GetCurrentTimeLimitInHour_With16_ShouldBeIn48Hours()
        {
            // Arrange
            const int timeLimitFor16InHour = 48;

            for (int i = 0; i < 8; i++)
            {
                _parkingMeter.AddCoin(2m);    
            }

            // Act
            var timeLimit = _parkingMeter.GetCurrentTimeLimitInHour();

            // Assert
            Assert.That(timeLimit, Is.EqualTo(timeLimitFor16InHour));
        }

        #endregion

        #region Parking Time Restrictions

        [Test]
        public void ParkingMeter_GetParkingLimitDate_InAugust_WithNoCoin_ShouldEndInSeptember()
        {
            // Arrange
            var dateInAugust = new DateTime(2000, 8, 1);
            var clock = new FakeClock(dateInAugust);
            var parkingMeter = new ParkingMeter(clock, _timeRulesConfiguration);

            // Act
            var limitDate = parkingMeter.GetParkingLimitDate();

            // Assert
            Assert.That(limitDate, Is.EqualTo(new DateTime(2000, 9, 1, 8, 0, 0)));
        }

        [Test]
        public void ParkingMeter_GetParkingLimitDate_OnFrenchHoliday_WithNotCoin_ShouldEndNextDay()
        {
            // Arrange
            var dateInAugust = new DateTime(2014, 12, 25);
            var clock = new FakeClock(dateInAugust);
            var parkingMeter = new ParkingMeter(clock, _timeRulesConfiguration);

            // Act
            var limitDate = parkingMeter.GetParkingLimitDate();

            // Assert
            Assert.That(limitDate, Is.EqualTo(new DateTime(2014, 12, 26, 8, 0, 0)));
        }

        [Test]
        public void ParkingMeter_GetParkingLimitDate_OnFreeTime_WithNotCoin_ShouldEndWhenNextTimeRestrictionStart()
        {
            // Arrange
            var dateInAugust = new DateTime(2015, 2, 23, 18, 0, 0);
            var clock = new FakeClock(dateInAugust);
            var parkingMeter = new ParkingMeter(clock, _timeRulesConfiguration);

            // Act
            var limitDate = parkingMeter.GetParkingLimitDate();

            // Assert
            Assert.That(limitDate, Is.EqualTo(new DateTime(2015, 2, 24, 8, 0, 0)));
        }

        #endregion
    }
}
