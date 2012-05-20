using System;
using HitThatLine.Core.Accounts;
using HitThatLine.Core.Utility;

namespace HitThatLine.Core.Discussion
{
    public class Post
    {
        public string Id { get; private set; }
        public string MarkdownText { get; private set; }
        public string DisplayText { get; private set; }
        public long UpVotes { get; private set; }
        public long DownVotes { get; private set; }
        public long NetVotes { get { return UpVotes - DownVotes; } }
        public double Score { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime? LastModified { get; private set; }
        public string UserAccountId { get; private set; }
        public string ThreadId { get; private set; }

        public Post(string markDownText, UserAccount userAccount)
        {
            MarkdownText = markDownText;
            DisplayText = markDownText.MarkdownTransform();
            UserAccountId = userAccount.Id;
            CreatedOn = DateTime.Now;
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
            if (UpVotes == 0) { Score = 0; return; }
            var totalVotes = UpVotes + DownVotes;
            var phat = 1.0 * UpVotes / totalVotes;
            Score = (phat + 1.96 * 1.96 / (2 * totalVotes) - 1.96 * Math.Sqrt((phat * (1 - phat) + 1.96 * 1.96 / (4 * totalVotes)) / totalVotes)) / (1 + 1.96 * 1.96 / totalVotes);
        }
    }
}