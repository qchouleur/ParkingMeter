namespace ParkingMeter.Logic.Tests
{
    using System;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class ParkingMeterTest
    {
        private ParkingMeter _parkingMeter;

        private FakeClock _clock;

        [SetUp]
        public void SetUp()
        {
            _clock = new FakeClock(DateTime.Now);
            _parkingMeter = new ParkingMeter(_clock);
        }

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
            decimal[] coins = {0.2m, 0.2m, 0.5m, 1m};

            // Act
            foreach (var coin in coins)
            {
                _parkingMeter.AddCoin(coin);
            }

            // Assert
            Assert.That(_parkingMeter.TotalAmountOfMoney, Is.EqualTo(coins.Sum()));
        }

        [Test]
        public void ParkingMeter_GetParkingLimitDate_WithNoCoin_ShouldBeCurrentDate()
        {
            // Act
            var limitDate = _parkingMeter.GetParkingLimitDate();

            // Assert
            Assert.That(limitDate, Is.EqualTo(_clock.Now));
        }

        [Test]
        public void ParkingMeter_GetParkingLimitDate_WithLessThanOne_ShouldBeCurrentDate()
        {
            // Arrange
            _parkingMeter.AddCoin(0.2m);
            _parkingMeter.AddCoin(0.5m);

            // Act
            var limitDate = _parkingMeter.GetParkingLimitDate();

            // Assert
            Assert.That(limitDate, Is.EqualTo(_clock.Now));
        }

        [Test]
        public void ParkingMeter_GetParkingLimitDate_WithOne_ShouldBeIn20Minutes()
        {
            // Arrange
            const int timeLimitForOneInMinutes = 20;
            _parkingMeter.AddCoin(1m);

            // Act
            var limitDate = _parkingMeter.GetParkingLimitDate();

            // Assert
            Assert.That(limitDate, Is.EqualTo(_clock.Now.AddMinutes(timeLimitForOneInMinutes)));
        }

        [Test]
        public void ParkingMeter_GetParkingLimitDate_WithTwo_ShouldBeIn1Hour()
        {
            // Arrange
            const int timeLimitForTwoInHour = 1;
            _parkingMeter.AddCoin(2m);

            // Act
            var limitDate = _parkingMeter.GetParkingLimitDate();

            // Assert
            Assert.That(limitDate, Is.EqualTo(_clock.Now.AddHours(timeLimitForTwoInHour)));
        }

        [Test]
        public void ParkingMeter_GetParkingLimitDate_With3_ShouldBeIn2Hours()
        {
            // Arrange
            const int timeLimitForTwoInHour = 2;
            _parkingMeter.AddCoin(2m);
            _parkingMeter.AddCoin(1m);

            // Act
            var limitDate = _parkingMeter.GetParkingLimitDate();

            // Assert
            Assert.That(limitDate, Is.EqualTo(_clock.Now.AddHours(timeLimitForTwoInHour)));
        }

        [Test]
        public void ParkingMeter_GetParkingLimitDate_With5_ShouldBeIn12Hours()
        {
            // Arrange
            const int timeLimitFor5InHour = 12;
            _parkingMeter.AddCoin(2m);
            _parkingMeter.AddCoin(2m);
            _parkingMeter.AddCoin(0.5m);
            _parkingMeter.AddCoin(0.5m);

            // Act
            var limitDate = _parkingMeter.GetParkingLimitDate();

            // Assert
            Assert.That(limitDate, Is.EqualTo(_clock.Now.AddHours(timeLimitFor5InHour)));
        }

        [Test]
        public void ParkingMeter_GetParkingLimitDate_With8_ShouldBeIn24Hours()
        {
            // Arrange
            const int timeLimitFor8InHour = 24;
            _parkingMeter.AddCoin(2m);
            _parkingMeter.AddCoin(2m);
            _parkingMeter.AddCoin(2m);
            _parkingMeter.AddCoin(2m);

            // Act
            var limitDate = _parkingMeter.GetParkingLimitDate();

            // Assert
            Assert.That(limitDate, Is.EqualTo(_clock.Now.AddHours(timeLimitFor8InHour)));
        }


        [Test]
        public void ParkingMeter_GetParkingLimitDate_With10_ShouldBeIn25Hours()
        {
            // Arrange
            const int timeLimitFor10InHour = 25;

            for (int i = 0; i < 5; i++)
            {
                _parkingMeter.AddCoin(2m);
            }

            // Act
            var limitDate = _parkingMeter.GetParkingLimitDate();

            // Assert
            Assert.That(limitDate, Is.EqualTo(_clock.Now.AddHours(timeLimitFor10InHour)));
        }

        [Test]
        public void ParkingMeter_GetParkingLimitDate_With16_ShouldBeIn48Hours()
        {
            // Arrange
            const int timeLimitFor16InHour = 48;

            for (int i = 0; i < 8; i++)
            {
                _parkingMeter.AddCoin(2m);    
            }

            // Act
            var limitDate = _parkingMeter.GetParkingLimitDate();

            // Assert
            Assert.That(limitDate, Is.EqualTo(_clock.Now.AddHours(timeLimitFor16InHour)));
        }
    }
}
