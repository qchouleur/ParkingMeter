using System;

namespace ParkingMeter.Logic.TimeRules
{
    public class TimePeriodRule
    {
        public int StartHour { get; private set; }
        public int EndHour { get; private set; }
        public int PeriodLenghtInHour { get { return EndHour - StartHour; } }

        public TimePeriodRule(int startHour, int endHour)
        {
            if (startHour < 0 || StartHour > 24)
            {
                throw new ArgumentException(string.Format(
                    "{0} is not a valid hour of the day", startHour));
            }

            if (endHour < 0 || startHour > 24)
            {
                throw new ArgumentException(string.Format(
                    "{0} is not a valid hour of the day", startHour));
            }

            if (endHour < startHour)
            {
                throw new ArgumentException(string.Format(
                    "The ending hour {0} should be after start hour {1}",
                    endHour, startHour));
            }

            EndHour = endHour;
            StartHour = startHour;
        }

        public bool Contains(TimeSpan timeOfDay)
        {
            return StartHour <= timeOfDay.TotalHours && EndHour > timeOfDay.TotalHours;
        }

        public TimeSpan TimeToReachPeriodEnd(TimeSpan timeOfDay)
        {
            if (!Contains(timeOfDay))
            {
                throw new ArgumentException(string.Format("{0} is not part of the period", timeOfDay));
            }

            return TimeSpan.FromHours(EndHour - timeOfDay.TotalHours);
        }

        public TimePeriodRule MergeWith(TimePeriodRule other)
        {
            return new TimePeriodRule(
                Math.Min(other.StartHour, StartHour), 
                Math.Max(other.EndHour, EndHour));
        }

        public bool OverlapsWith(TimePeriodRule other)
        {
            return Contains(other) || IsContainedBy(other) || SharePeriodWith(other);
        }

        private bool Contains(TimePeriodRule other)
        {
            return StartHour <= other.StartHour && EndHour >= other.EndHour;
        }

        private bool IsContainedBy(TimePeriodRule other)
        {
            return StartHour >= other.StartHour && EndHour <= other.EndHour;
        }

        private bool SharePeriodWith(TimePeriodRule other)
        {
            return ((other.StartHour == EndHour) || (other.EndHour == StartHour))
                   || (other.StartHour < StartHour && other.EndHour > StartHour && other.EndHour < EndHour)
                   || (other.StartHour > StartHour && other.EndHour > StartHour && other.StartHour < EndHour);
        }

        public override bool Equals(object other)
        {
            return Equals(other as TimePeriodRule);
        }

        public bool Equals(TimePeriodRule other)
        {
            if (other == null) return false;
            return StartHour == other.StartHour 
                && EndHour == other.EndHour;
        }

        public override int GetHashCode()
        {
            return new {StartHour, EndHour}.GetHashCode();
        }
    }
}