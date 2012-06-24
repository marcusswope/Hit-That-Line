using HitThatLine.Domain.Accounts;
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
                var author = new UserAccount { Username = "test", EmailHash = "testhash" };
                var thread = new DiscussionThread(title, body, "test,tags".Split(','), author);

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
                thread.AuthorProfilePictureUrl.ShouldEqual(author.ProfilePictureUrl);
                thread.AuthorUsername.ShouldEqual(author.Username);
            }
        }

        [TestFixture]
        public class AddPost
        {
            [Test]
            public void IncrementsPostCount()
            {
                var author = new UserAccount { Username = "test", EmailHash = "testhash" };
                var thread = new DiscussionThread("title", "body", "test,tags".Split(','), author);
                thread.PostCount.ShouldEqual(0);
                thread.AddPost("anotherUser");
                thread.PostCount.ShouldEqual(1);
                thread.LastActivity.ShouldBeWithinOneSecondFromNow();
                thread.LastActivityUsername.ShouldEqual("anotherUser");
            }
        }

        [TestFixture]
        public class Voting
        {
            [Test]
            public void IncrementsVoteCountAndRecalculatesTheScore()
            {
                var author = new UserAccount { Username = "test", EmailHash = "testhash" };
                var thread = new DiscussionThread("title", "body", "test,tags".Split(','), author);
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