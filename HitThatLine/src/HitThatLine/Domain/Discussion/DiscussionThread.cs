using System;
using System.Collections.Generic;
using HitThatLine.Domain.Accounts;
using HitThatLine.Utility;
using System.Linq;
using Newtonsoft.Json;

namespace HitThatLine.Domain.Discussion
{
    public class DiscussionThread
    {
        public string Id { get; set; }
        [JsonIgnore]
        public string UriId { get { return Id.Replace("threads/", string.Empty); } }

        public string Title { get; private set; }
        public string MarkdownBody { get; private set; }
        public string DisplayBody { get; private set; }
        public IList<string> Tags { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime LastActivity { get; private set; }
        public string LastActivityUsername { get; private set; }
        public long PostCount { get; private set; }
        public string AuthorProfilePictureUrl { get; private set; }
        public string AuthorUsername { get; private set; }

        public long UpVotes { get; private set; }
        public long DownVotes { get; private set; }
        public long NetVotes { get { return UpVotes - DownVotes; } }
        public double Score { get; private set; }

        public static string BuildDocumentKey(string uri)
        {
            return "threads/" + uri;
        }

        private DiscussionThread()
        { }

        public DiscussionThread(string title, string body, IEnumerable<string> tags, UserAccount author)
        {
            Id = "threads/";
            Title = title;
            MarkdownBody = body;
            DisplayBody = body.Transform();
            CreatedOn = DateTime.UtcNow;
            LastActivity = DateTime.UtcNow;
            UpVotes = 1;
            Tags = tags.ToList();
            AuthorUsername = author.Username;
            AuthorProfilePictureUrl = author.ProfilePictureUrl;
            calculateScore();
        }

        public void AddPost(string username)
        {
            PostCount++;
            LastActivity = DateTime.UtcNow;
            LastActivityUsername = username;
        }

        public void VoteUp()
        {
            UpVotes++;
            calculateScore();
        }

        public void VoteDown()
        {
            DownVotes++;
            calculateScore();
        }

        private void calculateScore()
        {
            //reddit algorithm
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var seconds = (CreatedOn - epoch).TotalSeconds - 1134028003;
            var order = Math.Log10(Math.Max(Math.Abs(NetVotes), 1));
            var sign = NetVotes > 0 ? 1 : NetVotes < 1 ? -1 : 0;
            Score = Math.Round(order + sign * seconds / 45000, 7);
        }
    }
}