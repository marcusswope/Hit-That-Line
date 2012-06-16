using System;
using HitThatLine.Web.Infrastructure;
using HitThatLine.Web.Infrastructure.Conventions.Attributes;
using HitThatLine.Web.Infrastructure.Conventions.Validation;
using NUnit.Framework;
using HitThatLine.Web.Tests.Utility;
using FluentValidation.TestHelper;

namespace HitThatLine.Web.Tests.Infrastructure.Conventions.Validation
{
    [TestFixture]
    public class ConventionBasedValidatorTest
    {
        private AllConventionsModel _model;
        private ConventionBasedValidator<AllConventionsModel> _validator;

        [SetUp]
        public void SetUp()
        {
            _model = new AllConventionsModel();
            _validator = new ConventionBasedValidator<AllConventionsModel>();
        }

        [Test]
        public void ValidatesRequiredFields()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.RequiredString, string.Empty);
            _validator.ShouldHaveValidationErrorFor(x => x.RequiredDate, default(DateTime));
            _validator.ShouldHaveValidationErrorFor(x => x.RequiredNullDate, null as DateTime?);
            _validator.ShouldHaveValidationErrorFor(x => x.RequiredInt, 0);
            _validator.ShouldHaveValidationErrorFor(x => x.RequiredNullInt, null as int?);
            _validator.ShouldHaveValidationErrorFor(x => x.RequiredLong, 0);
            _validator.ShouldHaveValidationErrorFor(x => x.RequiredNullLong, null as long?);
            _validator.ShouldHaveValidationErrorFor(x => x.Child, null as ChildConventionalModel);
        }

        [Test]
        public void ValidatesMinLength()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Password, "asdf");
        }

        [Test]
        public void ValidatesMaxLength()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Username, "asdfasdf");
        }

        [Test]
        public void ValidatesAllDatesMustBeBeforeToday()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.BirthDate, DateTime.Today.AddDays(1));
        }

        [Test]
        public void ValidatesEmailAddressFormat()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.EmailAddress, "google.com");
        }

        [Test]
        public void ValidatesZipCodeMustBeBetween5And9Digits()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.ZipCode, "1234");
            _validator.ShouldHaveValidationErrorFor(x => x.ZipCode, "1234567890");
        }

        [Test]
        public void ValidatesPhoneMustBeExactly10Digits()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.PhoneNumber, "12345678900");
            _validator.ShouldHaveValidationErrorFor(x => x.PhoneNumber, "123456789");
        }

        [Test]
        public void ValidatesSSNMustBeExactly9Digits()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.SocialSecurityNumber, "98765432");
            _validator.ShouldHaveValidationErrorFor(x => x.SocialSecurityNumber, "9876543211");
        }

        [Test]
        public void SuccessfulValidation()
        {
            _validator.Validate(_model).IsValid.ShouldBeTrue();
        }
    }

    public class AllConventionsModel : IValidatedCommand
    {
        public AllConventionsModel()
        {
            RequiredString = "populated";
            RequiredDate = DateTime.Now;
            RequiredNullDate = DateTime.Now;
            RequiredInt = 2;
            RequiredNullInt = 3;
            RequiredLong = 4;
            RequiredNullLong = 5;
            Password = "asdfasdf";
            Username = "asdf";
            Username = "asdf";
            BirthDate = new DateTime(1980, 1, 1);
            EmailAddress = "abcd@abcd.com";
            ZipCode = "72201";
            PhoneNumber = "3215649874";
            SocialSecurityNumber = "321654987";
            Child = new ChildConventionalModel();
        }

        [Required]
        public string RequiredString { get; set; }
        [Required]
        public DateTime RequiredDate { get; set; }
        [Required]
        public DateTime? RequiredNullDate { get; set; }
        [Required]
        public int RequiredInt { get; set; }
        [Required]
        public int? RequiredNullInt { get; set; }
        [Required]
        public long RequiredLong { get; set; }
        [Required]
        public long? RequiredNullLong { get; set; }
        [Required]
        public ChildConventionalModel Child { get; set; }

        [MinLength(6)]
        public string Password { get; set; }
        [MaxLength(6)]
        public string Username { get; set; }

        public DateTime BirthDate { get; set; }
        public string EmailAddress { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string SocialSecurityNumber { get; set; }

        public object TransferToOnFailed
        {
            get { return null; }
        }
    }

    public class ChildConventionalModel
    {
    }
}