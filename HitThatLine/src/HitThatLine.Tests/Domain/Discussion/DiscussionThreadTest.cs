using HitThatLine.Domain.Discussion;
using NUnit.Framework;
using HitThatLine.Tests.Utility;

namespace HitThatLine.Tests.Domain.Discussion
{
    public class DiscussionThreadTest
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void InitializesPropertiesCorrectly()
            {
                var title = "New Thread";
                var body = "This is a body. And this is **really important**.";
                var thread = new DiscussionThread(title, body, "test,tags".Split(','));
                
                thread.Id.ShouldEqual("threads/");
                thread.Title.ShouldEqual(title);
                thread.MarkdownBody.ShouldEqual(body);
                thread.DisplayBody.ShouldEqual("<p>This is a body. And this is <strong>really important</strong>.</p>\n");
                thread.CreatedOn.ShouldBeWithinOneSecondFromNow();
                thread.LastActivity.ShouldBeWithinOneSecondFromNow();
                thread.UpVotes.ShouldEqual(1);
                thread.Score.ShouldBeGreaterThan(0);
                thread.Tags[0].ShouldEqual("test");
                thread.Tags[1].ShouldEqual("tags");
            }
        }

        [TestFixture]
        public class AddPost
        {
            [Test]
            public void IncrementsPostCount()
            {
                var thread = new DiscussionThread("title", "body", "test,tags".Split(','));
                thread.PostCount.ShouldEqual(0);
                thread.AddPost();
                thread.PostCount.ShouldEqual(1);
            }
        }

        [TestFixture]
        public class Voting
        {
            [Test]
            public void IncrementsVoteCountAndRecalculatesTheScore()
            {
                var thread = new DiscussionThread("title", "body", "test,tags".Split(','));
                var originalScore = thread.Score;
                thread.UpVotes.ShouldEqual(1);
                thread.DownVotes.ShouldEqual(0);
                
                thread.VoteUp();
                thread.UpVotes.ShouldEqual(2);
                thread.DownVotes.ShouldEqual(0);
                thread.Score.ShouldBeGreaterThan(originalScore);

                thread.VoteDown();
                thread.UpVotes.ShouldEqual(2);
                thread.DownVotes.ShouldEqual(1);
                thread.Score.ShouldEqual(originalScore);

                thread.NetVotes.ShouldEqual(1);
            }
        }
    }
}