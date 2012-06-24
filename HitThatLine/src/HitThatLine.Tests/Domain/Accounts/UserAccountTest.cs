using HitThatLine.Domain.Accounts;
using NUnit.Framework;
using HitThatLine.Tests.Utility;

namespace HitThatLine.Tests.Domain.Accounts
{
    [TestFixture]
    public class UserAccountTest
    {
        [TestFixture]
        public class ProfilePicture
        {
            [Test]
            public void ReturnsGravatarUrlWithEmailHash()
            {
                var account = new UserAccount { EmailHash = "someHash" };
                account.ProfilePictureUrl.ShouldEqual("http://www.gravatar.com/avatar/someHash?d=identicon&r=pg&s=60");
            }
        }
    }
}