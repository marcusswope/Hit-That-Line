using System;
using System.Collections.Generic;
using HitThatLine.Utility;

namespace HitThatLine.Endpoints.Home.Models
{
    public class HomeRequest
    {
        public TimeSpan TimeZoneOffset { get; set; }
        public List<ThreadSummaryViewModel> Threads { get; set; }
    }

    public class HomeViewModel : HomeRequest
    {

    }

    public class ThreadSummaryViewModel
    {
        public string Title { get; set; }
        public string DisplayBody { get; set; }
        public string AuthorProfilePictureUrl { get; set; }
        public string AuthorUsername { get; set; }
        public long PostCount { get; set; }
        public long NetVotes { get; set; }
        public DateTime CreatedOn { get; set; }
        public TimeSpan TimeZoneOffset { get; set; }
        public string TimePostedDifference { get { return CreatedOn.DisplayDateDifference(TimeZoneOffset); } }
        public List<string> Tags { get; set; }
    }
}