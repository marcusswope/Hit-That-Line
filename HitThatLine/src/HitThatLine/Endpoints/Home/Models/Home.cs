using System;
using System.Collections.Generic;
using HitThatLine.Endpoints.Thread.Models;
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
        public int TotalPages { get; set; }
    }

    public class ThreadSummaryViewModel
    {
        public string Title { get; set; }
        public string DisplayBody { get; set; }
        public string AuthorProfilePictureUrl { get; set; }
        public string AuthorUsername { get; set; }
        public int PostCount { get; set; }
        public int ViewCount { get; set; }
        public int NetVotes { get; set; }
        public DateTime LastActivity { get; set; }
        public string LastActivityUsername { get; set; }
        public DateTime CreatedOn { get; set; }
        public TimeSpan TimeZoneOffset { get; set; }
        public string TimePostedDifference { get { return CreatedOn.DisplayDateDifference(TimeZoneOffset); } }
        public string LastActivityDifference { get { return LastActivity.DisplayDateDifference(TimeZoneOffset); } }
        public List<string> Tags { get; set; }

        public string UriId { get; set; }
        public ViewThreadRequest ViewThreadRequest { get { return new ViewThreadRequest { UriId = UriId, Title = Title }; } }
    }
}