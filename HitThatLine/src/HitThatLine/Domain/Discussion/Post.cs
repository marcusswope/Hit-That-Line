using System;
using HitThatLine.Domain.Accounts;
using HitThatLine.Utility;

namespace HitThatLine.Domain.Discussion
{
    public class Post
    {
        public string Id { get; set; }
        public string MarkdownText { get; private set; }
        public string DisplayText { get; private set; }
        public long UpVotes { get; private set; }
        public long DownVotes { get; private set; }
        public long NetVotes { get { return UpVotes - DownVotes; } }
        public double Score { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime LastActivity { get; private set; }
        public string Username { get; private set; }
        public string ThreadId { get; private set; }

        private Post(){}

        public Post(string markDownText, string discussionThreadId, UserAccount userAccount)
        {
            MarkdownText = markDownText;
            DisplayText = markDownText.Transform();
            ThreadId = discussionThreadId;
            Username = userAccount.Username;
            CreatedOn = DateTime.UtcNow;
            LastActivity = DateTime.UtcNow;
            UpVotes = 1;
            calculateScore();
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
            if (UpVotes == 0) { Score = 0; return; }
            var totalVotes = UpVotes + DownVotes;
            var phat = 1.0 * UpVotes / totalVotes;
            Score = (phat + 1.96 * 1.96 / (2 * totalVotes) - 1.96 * Math.Sqrt((phat * (1 - phat) + 1.96 * 1.96 / (4 * totalVotes)) / totalVotes)) / (1 + 1.96 * 1.96 / totalVotes);
        }
    }
}