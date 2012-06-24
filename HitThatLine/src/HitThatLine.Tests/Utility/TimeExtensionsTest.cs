using System;
using NUnit.Framework;
using HitThatLine.Utility;

namespace HitThatLine.Tests.Utility
{
    [TestFixture]
    public class TimeExtensionsTest
    {
        [Test]
        public void OutputsMonthDayYearAndTimeFor250DaysAgo()
        {
            var originalDate = new DateTime(2011, 1, 1, 13, 30, 25);
            var difference = originalDate.DisplayDateDifference(new DateTime(2012, 1, 1));
            difference.ShouldEqual("Jan 1 '11 at 1:30pm");
        }

        [Test]
        public void OutputsMonthDayAndTimeFor6DaysAgo()
        {
            var originalDate = new DateTime(2012, 1, 1, 13, 30, 25);
            var difference = originalDate.DisplayDateDifference(new DateTime(2012, 1, 10));
            difference.ShouldEqual("Jan 1 at 1:30pm");
        }

        [Test]
        public void OutputsDayOfWeekAndTimeForLessThanAWeekAgo()
        {
            var originalDate = new DateTime(2012, 1, 1, 13, 30, 25);
            var difference = originalDate.DisplayDateDifference(new DateTime(2012, 1, 3));
            difference.ShouldEqual("Sun at 1:30pm");
        }

        [Test]
        public void OutputsYesterdayAndDateForLessThanADayAgo()
        {
            var originalDate = new DateTime(2012, 1, 1, 13, 30, 25);
            var difference = originalDate.DisplayDateDifference(new DateTime(2012, 1, 2));
            difference.ShouldEqual("Yesterday at 1:30pm");
        }

        [Test]
        public void OutputsNumberOfHoursAgoForSameDay()
        {
            var originalDate = new DateTime(2012, 1, 1, 13, 30, 25);
            var difference = originalDate.DisplayDateDifference(new DateTime(2012, 1, 1, 18, 30, 25));
            difference.ShouldEqual("5 hours ago at 1:30pm");
            var oneHourDifference = originalDate.DisplayDateDifference(new DateTime(2012, 1, 1, 14, 30, 25));
            oneHourDifference.ShouldEqual("1 hour ago at 1:30pm");
        }

        [Test]
        public void OutputsNumberOfMinutesAgoForLessThanOneHour()
        {
            var originalDate = new DateTime(2012, 1, 1, 13, 30, 25);
            var difference = originalDate.DisplayDateDifference(new DateTime(2012, 1, 1, 13, 40, 25));
            difference.ShouldEqual("10 minutes ago");
            var oneMinuteDifference = originalDate.DisplayDateDifference(new DateTime(2012, 1, 1, 13, 31, 25));
            oneMinuteDifference.ShouldEqual("1 minute ago");
        }

        [Test]
        public void OutputsNumberOfSecondsAgoForLessThanOneMinute()
        {
            var originalDate = new DateTime(2012, 1, 1, 13, 30, 25);
            var difference = originalDate.DisplayDateDifference(new DateTime(2012, 1, 1, 13, 30, 35));
            difference.ShouldEqual("10 seconds ago");
            var oneSecondDifference = originalDate.DisplayDateDifference(new DateTime(2012, 1, 1, 13, 30, 26));
            oneSecondDifference.ShouldEqual("1 second ago");
        }
    }
}