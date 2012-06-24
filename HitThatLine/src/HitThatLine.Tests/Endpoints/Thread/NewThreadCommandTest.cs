using HitThatLine.Endpoints.Thread.Models;
using NUnit.Framework;
using HitThatLine.Tests.Utility;

namespace HitThatLine.Tests.Endpoints.Thread
{
    [TestFixture]
    public class NewThreadCommandTest
    {
        [Test]
        public void ShouldParseCommaDelimitedTagsAndSpaceDelimitedTagInput()
        {
            var command = new NewThreadCommand { Tags = "tags,test", TagInput = "hogs baseball" };
            command.AllTagsEntered.ShouldContain("tags");
            command.AllTagsEntered.ShouldContain("test");
            command.AllTagsEntered.ShouldContain("hogs");
            command.AllTagsEntered.ShouldContain("baseball");
        }
    }
}