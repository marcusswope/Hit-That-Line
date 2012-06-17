using System;
using HitThatLine.Endpoints.Account.Models;
using HitThatLine.Endpoints.Account.Validation;
using HitThatLine.Tests.Utility;
using NUnit.Framework;
using Raven.Abstractions.Extensions;
using FluentValidation.TestHelper;

namespace HitThatLine.Tests.Endpoints.Account.Validation
{
    [TestFixture]
    public class RegisterCommandValidatorTest : RavenTestBase
    {
        [Test]
        public void ValidatesRequiredFields()
        {
            var validator = new RegisterCommandValidator(Session);
            
            validator.ShouldHaveValidationErrorFor(x => x.Username, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.Password, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.Password, "asdf");
            validator.ShouldHaveValidationErrorFor(x => x.ConfirmPassword, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.EmailAddress, string.Empty);
        }

        [Test]
        public void ValidatesDuplicateEmails()
        {
            var validator = new RegisterCommandValidator(Session);

            validator.ShouldHaveValidationErrorFor(x => x.EmailAddress, DefaultUser.EmailAddress);
        }

        [Test]
        public void ValidatesDuplicateUsernames()
        {
            var validator = new RegisterCommandValidator(Session);

            validator.ShouldHaveValidationErrorFor(x => x.Username, DefaultUser.Username);
        }

        [Test]
        public void ValidatesConfirmPasswordMatch()
        {
            var validator = new RegisterCommandValidator(Session);
            var command = validCommand();
            command.ConfirmPassword = "different";

            validator.ShouldHaveValidationErrorFor(x => x.ConfirmPassword, command);
        }

        [Test]
        public void SuccessfulValidation()
        {
            var validator = new RegisterCommandValidator(Session);
            var command = validCommand();

            var validationResult = validator.Validate(command);
            validationResult.Errors.ForEach(Console.WriteLine);
            validationResult.IsValid.ShouldBeTrue();
        }

        private static RegisterCommand validCommand()
        {
            var command = new RegisterCommand
            {
                Username = "userThatDoesntExist",
                Password = "password",
                ConfirmPassword = "password",
                EmailAddress = "asdf@asdf.com",
            };
            return command;
        }
    }
}