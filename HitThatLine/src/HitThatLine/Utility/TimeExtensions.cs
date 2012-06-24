using System;
using FubuCore;

namespace HitThatLine.Utility
{
    public static class TimeExtensions
    {
        public static string DisplayDateDifference(this DateTime originalUtcDate, TimeSpan timeZoneOffset)
        {
            var originalLocalTime = originalUtcDate.Add(timeZoneOffset);
            var currentClientTime = DateTime.UtcNow.Add(timeZoneOffset);
            return originalLocalTime.DisplayDateDifference(currentClientTime);
        }

        public static string DisplayDateDifference(this DateTime originalLocalTime, DateTime currentClientTime)
        {
            var localDifference = currentClientTime.Subtract(originalLocalTime);
            if (localDifference.Days > 250)
            {
                return "{0} '{1} at {2}".ToFormat(originalLocalTime.ToString("MMM d"), originalLocalTime.ToString("yy"), originalLocalTime.TimeOfDay());
            }
            if (localDifference.Days > 6)
            {
                return "{0} at {1}".ToFormat(originalLocalTime.ToString("MMM d"), originalLocalTime.TimeOfDay());
            }
            if (localDifference.Days > 0)
            {
                return "{0} at {1}".ToFormat(originalLocalTime.ToString("ddd"), originalLocalTime.TimeOfDay());
            }
            if (localDifference.Hours > 0)
            {
                if (currentClientTime.DayOfYear == originalLocalTime.DayOfYear)
                {
                    var hourOrHours = localDifference.Hours > 1 ? "hours" : "hour";
                    return "{0} {1} ago at {2}{3}".ToFormat(localDifference.Hours, hourOrHours, originalLocalTime.ToString("h:mm"), originalLocalTime.AmPm());
                }
                return "Yesterday at {0}".ToFormat(originalLocalTime.TimeOfDay());
            }
            if (localDifference.Minutes != 0)
            {
                var minuteOrMinutes = localDifference.Minutes > 1 ? "minutes" : "minute";
                return "{0} {1} ago".ToFormat(localDifference.Minutes, minuteOrMinutes);
            }
            var secondOrSeconds = localDifference.Seconds > 1 ? "seconds" : "second";
            return "{0} {1} ago".ToFormat(localDifference.Seconds, secondOrSeconds);
        }

        public static string AmPm(this DateTime date)
        {
            return date.ToString("tt").ToLower();
        }

        public static string TimeOfDay(this DateTime date)
        {
            return date.ToString("h:mm") + date.AmPm();
        }
    }
}