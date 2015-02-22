using System;
using System.Linq;
using ParkingMeter.Logic.TimeRules;

namespace ParkingMeter.Logic.Tests.TimeRules
{
    using NUnit.Framework; 

    [TestFixture]
    public class DayRuleTest
    {
        [Test]
        public void DayRule_Constructor_ShouldMergeTimePeriods()
        {
            // Arrange
            var firstTimePeriod = new TimePeriodRule(1, 2);
            var secondTimePeriod = new TimePeriodRule(3, 4);
            var thirdTimePeriod = new TimePeriodRule(2, 5);
            var notOverlappingTimePeriod = new TimePeriodRule(22, 24);

            // Act
            var dayRule = new DayRule(DayOfWeek.Monday, new []{firstTimePeriod, secondTimePeriod, thirdTimePeriod, notOverlappingTimePeriod});
        
            // Assert
            Assert.That(dayRule.TimePeriodRules.Count(), Is.EqualTo(2));
            Assert.That(dayRule.TimePeriodRules.First(), Is.EqualTo(new TimePeriodRule(1, 5)));
            Assert.That(dayRule.TimePeriodRules.Skip(1).First(), Is.EqualTo(notOverlappingTimePeriod));
        }
    }
}
