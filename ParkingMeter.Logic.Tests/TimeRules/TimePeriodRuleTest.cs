using ParkingMeter.Logic.TimeRules;
using NUnit.Framework;
using System;

namespace ParkingMeter.Logic.Tests.TimeRules
{
    [TestFixture]
    public class TimePeriodRuleTest
    {
        [TestCase(1, 24, 2, 3, TestName = "TimePeriodRule_OverlapsWith_ContainedPeriod_ShouldReturnTrue", Result = true)]
        [TestCase(2, 3, 1, 24, TestName = "TimePeriodRule_OverlapsWith_ContainingPeriod_ShouldReturnTrue", Result = true)]
        [TestCase(2, 4, 3, 5, TestName = "TimePeriodRule_OverlapsWith_WithSharingEndingPeriod_ShouldReturnTrue", Result = true)]
        [TestCase(3, 5, 2, 4, TestName = "TimePeriodRule_OverlapsWith_WithSharingStartingPeriod_ShouldReturnTrue", Result = true)]
        [TestCase(1, 2, 2, 4, TestName = "TimePeriodRule_OverlapsWith_WithSharingBoundaryEndPeriod_ShouldReturnTrue", Result = true)]
        [TestCase(1, 2, 23, 24, TestName = "TimePeriodRule_OverlapsWith_WithDisjointPeriod_ShouldReturnFalse", Result = false)]
        public bool TimePeriodRule_OverlapsWith_ShouldOverlappingIndication(
            int firstPeriodStartHour, int firstPeriodEndHour,
            int secondPeriodStartHour, int secondPeriodEndHour)
        {
            // Arrange
            var firstTimePeriod = new TimePeriodRule(firstPeriodStartHour, firstPeriodEndHour);
            var secondTimePeriod = new TimePeriodRule(secondPeriodStartHour, secondPeriodEndHour);

            // Act
            return firstTimePeriod.OverlapsWith(secondTimePeriod);
        }

        [TestCase(12, 0, 24, TestName = "ContainsTime_WithHourOfTheDayInsidePeriod_ShouldReturnTrue", Result = true)]
        [TestCase(20, 0, 0, TestName = "ContainsTime_WithHourOfTheDayCorrespondingToUpperBoundPeriod_ShouldReturnFalse", Result = false)]
        [TestCase(1, 0, 0, TestName = "ContainsTime_WithHourOfTheDayCorrespondingToLowerBoundPeriod_ShouldReturnTrue", Result = true)]
        [TestCase(21, 0, 0, TestName = "ContainsTime_WithHourOfTheDayOutsidePeriod_ShouldReturnFalse", Result = false)]
        public bool TimePeriodRule_ContainsTime_ShouldIndicatesIfPeriodContainsTime(int hours, int minutes, int seconds)
        {
            // Arrange
            var timePeriod = new TimePeriodRule(1, 20);
            
            // Act
            return timePeriod.Contains(new TimeSpan(hours, minutes, seconds));
        }
    }
}

