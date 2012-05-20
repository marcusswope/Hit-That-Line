using System;
using System.Collections.Generic;
using HitThatLine.Core.Utility;

namespace HitThatLine.Core.Discussion
{
    public class DiscussionThread
    {
        public string Id { get; set; }

        public string Title { get; set; }
        public string MarkdownBody { get; set; }
        public string DisplayBody { get; set; }
        public IList<string> Tags { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastActivity { get; set; }
        public long PostCount { get; private set; }

        public long UpVotes { get; set; }
        public long DownVotes { get; set; }
        public long NetVotes { get { return UpVotes - DownVotes; } }
        public double Score { get; set; }

        public DiscussionThread(string title, string body)
        {
            Title = title;
            MarkdownBody = body;
            DisplayBody = body.MarkdownTransform();
            CreatedOn = DateTime.Now;
            LastActivity = DateTime.Now;
            calculateScore();
        }

        public void AddPost()
        {
            PostCount++;
        }

        public void VoteUp()
        {
            UpVotes++;
            calculateScore();
        }

        public void VoteDown()
        {
            UpVotes++;
            calculateScore();
        }

        private void calculateScore()
        {
            var epoch = new DateTime(1970, 1, 1);
            var seconds = (CreatedOn - epoch).TotalSeconds - 1134028003;
            var order = Math.Log10(Math.Max(Math.Abs(NetVotes), 1));
            var sign = NetVotes > 0 ? 1 : NetVotes < 1 ? -1 : 0;
            Score = Math.Round(order + sign * seconds / 45000, 7);
        }
    }
}