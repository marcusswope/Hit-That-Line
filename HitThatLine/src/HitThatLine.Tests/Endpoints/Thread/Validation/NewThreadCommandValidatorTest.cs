using HitThatLine.Endpoints.Thread.Models;
using HitThatLine.Endpoints.Thread.Validation;
using NUnit.Framework;
using FluentValidation.TestHelper;

namespace HitThatLine.Tests.Endpoints.Thread
{
    [TestFixture]
    public class NewThreadCommandValidatorTest
    {
        [Test]
        public void TagsOrTagInputMustBeSupplied()
        {
            var validator = new NewThreadCommandValidator();
            var command = new NewThreadCommand();

            validator.ShouldHaveValidationErrorFor(x => x.TagInput, command);

            command.Tags = "test";

            validator.ShouldNotHaveValidationErrorFor(x => x.TagInput, command);

            command.Tags = null;
            command.TagInput = "test";

            validator.ShouldNotHaveValidationErrorFor(x => x.TagInput, command);

            command.Tags = "test";

            validator.ShouldNotHaveValidationErrorFor(x => x.TagInput, command);
        }
    }
}