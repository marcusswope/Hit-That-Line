using HitThatLine.Web.Endpoints.Account.Models;
using HitThatLine.Web.Endpoints.Account.Validation;
using HitThatLine.Web.Tests.Utility;
using NUnit.Framework;
using FluentValidation.TestHelper;

namespace HitThatLine.Web.Tests.Endpoints.Account.Validation
{
    [TestFixture]
    public class LoginCommandValidatorTest : RavenTestBase
    {
        [Test]
        public void ValidatesRequiredFields()
        {
            var validator = new LoginCommandValidator(Session);

            validator.ShouldHaveValidationErrorFor(x => x.Username, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.Password, string.Empty);
        }

        [Test]
        public void ValidatesUserExists()
        {
            var validator = new LoginCommandValidator(Session);
            var command = new LoginCommand { Username = "userThatDoesntExist", Password = DefaultUser.Password };

            validator.ShouldHaveValidationErrorFor(x => x.Username, command);
        }

        [Test]
        public void ValidatesPasswordCorrect()
        {
            var validator = new LoginCommandValidator(Session);
            var command = new LoginCommand { Username = DefaultUser.Username, Password = "wrongPassword" };
            
            validator.ShouldHaveValidationErrorFor(x => x.Password, command);
        }

        [Test]
        public void SuccessfulValidation()
        {
            var validator = new LoginCommandValidator(Session);
            var command = new LoginCommand { Username = DefaultUser.Username, Password = DefaultUser.Password };

            validator.Validate(command).IsValid.ShouldBeTrue();
        }
    }
}