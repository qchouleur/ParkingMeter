using System;
using System.Collections.Generic;
using System.Linq;
using ParkingMeter.Logic.TimeRules;

namespace ParkingMeter.Logic.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ParkingRestrictionsTest
    {
        private TimeRulesConfiguration _configuration;

        private ParkingRestrictions _restrictions;

        [SetUp]
        public void SetUp()
        {
            _configuration = new TimeRulesConfiguration();
            _configuration.Add(new DayRule(DayOfWeek.Monday, new[] { new TimePeriodRule(16, 18), new TimePeriodRule(19, 22) }));

            _restrictions = new ParkingRestrictions(_configuration);
        }

        [Test]
        public void GetNextTimePeriodRule_WithCurrentDate_ShouldGiveNextTimePeriodRuleInMultipleDays()
        {
            // Arrange
            var initialDate = new DateTime(2015, 2, 20);

            // Act
            var timePeriodRule = _restrictions.GetNextMatchingTimePeriodRule(initialDate);

            // Assert
            Assert.That(timePeriodRule, Is.EqualTo(_configuration.DayRules.First().TimePeriodRules.First()));
        }

        [Test]
        public void GetNextTimePeriodRule_WithDateBetweenTwoTimePeriod_ShouldGiveLastTimePeriod()
        {
            // Arrange
            var initialDate = new DateTime(2015, 2, 23, 18, 30, 0);

            // Act
            var timePeriodRule = _restrictions.GetNextMatchingTimePeriodRule(initialDate);

            // Assert
            Assert.That(timePeriodRule, Is.EqualTo(_configuration.DayRules.First().TimePeriodRules.Skip(1).First()));
        }

        [Test]
        public void GetNextTimeRestrictionStartDate_WithCurrentDate_ShouldGiveNextTimeRestrictionStartDate()
        {
            // Arrange
            var initialDate = new DateTime(2015, 2, 20);
            var expectedTimeRestrictionStartDate = new DateTime(2015, 2, 23, 16, 0, 0);

            // Act
            var actualTimeRestrictionStartDate = _restrictions.GetNextTimeRestrictionStartDate(initialDate);

            // Assert
            Assert.That(actualTimeRestrictionStartDate, Is.EqualTo(expectedTimeRestrictionStartDate));
        }

        [Test]
        public void IsInFreeDay_WithDayInFrenchPublicHolidays_ShouldReturnTrue()
        {
            // Arrange
            var frenchHolidays = new []
            {
                new DateTime(2014, 1, 1), new DateTime(2014, 4, 21), new DateTime(2014, 5, 1), new DateTime(2014, 5, 8),
                new DateTime(2014, 5, 29), new DateTime(2014, 8, 29), new DateTime(2014, 6, 9),
                new DateTime(2014, 7, 17), new DateTime(2014, 8, 15), new DateTime(2014, 11, 1),
                new DateTime(2014, 11, 11), new DateTime(2014, 12, 25)
            };
            
            // Act 
            var freeDayResult = frenchHolidays.Select(holiday => _restrictions.IsInFreeDay(holiday));
            
            // Assert
            Assert.IsTrue(freeDayResult.All(result => result));
        }
    }
}
