using System;
using NUnit.Framework;
using ParkingMeter.Logic.TimeRules;

namespace ParkingMeter.Logic.Tests
{
    [TestFixture]
    public class TimeLimitRuleTest
    {
        private TimeRulesConfiguration _configurationWithOnlyMondayRule;

        [SetUp]
        public void SetUp()
        {
            _configurationWithOnlyMondayRule = new TimeRulesConfiguration();
            _configurationWithOnlyMondayRule.Add(new DayRule(DayOfWeek.Monday, new[] { new TimePeriodRule(10, 11) }));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TimeLimitRule_Constructor_WithEmptyConfiguration_ShouldThrow()
        {
            // Arrange
            var emptyConfiguration = new TimeRulesConfiguration();

            // Act
            new TimeLimitRule(new Clock(), emptyConfiguration);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TimeLimitRule_Constructor_WithNoClock_ShouldThrow()
        {
            // Act
            new TimeLimitRule(null, _configurationWithOnlyMondayRule);
        }

        [Test]
        public void TimeLimitRule_ComputeEndingTimeDate_WithNoTimeToSpendAndMatchingTimeRule_ShouldGiveCurrentDate()
        {
            // Arrange
            var currentDate = new DateTime(2015, 2, 23, 10, 30, 0);
            var clock = new FakeClock(currentDate);
            var timeLimitRule = new TimeLimitRule(clock, _configurationWithOnlyMondayRule);

            // Act
            var actualEndingDate = timeLimitRule.ComputeEndingTimeDate(TimeSpan.Zero);

            // Assert
            Assert.That(actualEndingDate, Is.EqualTo(currentDate));
        }

        [Test]
        public void TimeLimitRule_ComputeEndingTimeDate_WithFreeMonthRuleAndNoTimeToSpend_NextTimePeriodRuleStartDate()
        {
            // Arrange
            _configurationWithOnlyMondayRule.Add(new FreeMonthRule(8));
            var date = new DateTime(2014, 8, 1);
            var clock = new FakeClock(date);
            var timeLimitRule = new TimeLimitRule(clock, _configurationWithOnlyMondayRule);

            // Act
            var endingDate = timeLimitRule.ComputeEndingTimeDate(TimeSpan.Zero);

            // Assert
            Assert.That(endingDate, Is.EqualTo(new DateTime(2014, 9, 1, 10, 0, 0)));
        }

        [Test]
        public void TimeLimitRule_ComputeEndingTimeDate_WithTimeToSpendLessThanPeriod_ShouldGiveValidEndDate()
        {
            // Arrange
            var initialDate = new DateTime(2015, 2, 23, 10, 30, 0);
            var timeToSpend = new TimeSpan(0, 0, 30, 0);
            var clock = new FakeClock(initialDate);
            var timeLimitRule = new TimeLimitRule(clock, _configurationWithOnlyMondayRule);
            
            // Act
            var endingDate = timeLimitRule.ComputeEndingTimeDate(timeToSpend);

            // Assert
            Assert.That(endingDate, Is.EqualTo(new DateTime(2015, 2, 23, 11, 0, 0)));
        }

        [Test]
        public void TimeLimitRule_ComputeEndingTimeDate_WithTimeToSpendEqualToPeriod_ShouldGiveValidEndDate()
        {
            // Arrange
            var initialDate = new DateTime(2015, 2, 23, 10, 00, 0);
            var timeToSpend = new TimeSpan(0, 1, 0, 0);
            var clock = new FakeClock(initialDate);
            var timeLimitRule = new TimeLimitRule(clock, _configurationWithOnlyMondayRule);

            // Act
            var endingDate = timeLimitRule.ComputeEndingTimeDate(timeToSpend);

            // Assert
            Assert.That(endingDate, Is.EqualTo(new DateTime(2015, 2, 23, 11, 0, 0)));
        }

        [Test]
        public void TimeLimitRule_ComputeEndingTimeDate_WithTimeToSpendGreaterThanPeriod_ShouldGiveEndDateNextMonday()
        {
            // Arrange
            var initialDate = new DateTime(2015, 2, 23, 10, 30, 0);
            var timeToSpend = new TimeSpan(0, 1, 30, 0);
            var clock = new FakeClock(initialDate);
            var timeLimitRule = new TimeLimitRule(clock, _configurationWithOnlyMondayRule);

            // Act
            var endingDate = timeLimitRule.ComputeEndingTimeDate(timeToSpend);

            // Assert
            Assert.That(endingDate, Is.EqualTo(new DateTime(2015, 3, 2, 11, 0, 0)));
        }

        [Test]
        public void TimeLimitRule_ComputeEndingTimeDate_WithMultipleTimePeriodRule_ShouldGiveEndDate()
        {
            // Arrange
            var initialDate = new DateTime(2015, 2, 22);
            var timeToSpend = new TimeSpan(0, 2, 0, 0);
            var configuration = new TimeRulesConfiguration();
            configuration.Add(new DayRule(DayOfWeek.Monday, new []{ new TimePeriodRule(10, 11), new TimePeriodRule(14, 15) }));
            var clock = new FakeClock(initialDate);
            var timeLimitRule = new TimeLimitRule(clock, configuration);

            // Act
            var actualEndingDate = timeLimitRule.ComputeEndingTimeDate(timeToSpend);

            // Assert
            Assert.That(actualEndingDate, Is.EqualTo(new DateTime(2015, 2, 23, 15, 0, 0)));
        }

        [Test]
        public void TimeLimitRule_ComputeEndingTimeDate_WithMultipleTimeDayRule_ShouldGiveEndDate()
        {
            // Arrange
            var initialDate = new DateTime(2015, 2, 22);
            var timeToSpend = new TimeSpan(0, 4, 0, 0);
            var configuration = new TimeRulesConfiguration();
            configuration.Add(new DayRule(DayOfWeek.Monday, new[] { new TimePeriodRule(10, 11) }));
            configuration.Add(new DayRule(DayOfWeek.Tuesday, new[] { new TimePeriodRule(10, 12), new TimePeriodRule(15, 17) }));
            var clock = new FakeClock(initialDate);
            var timeLimitRule = new TimeLimitRule(clock, configuration);

            // Act
            var actualEndingDate = timeLimitRule.ComputeEndingTimeDate(timeToSpend);

            // Assert
            Assert.That(actualEndingDate, Is.EqualTo(new DateTime(2015, 2, 24, 16, 0, 0)));
        }
    
    }
}
