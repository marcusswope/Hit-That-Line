using FluentValidation;
using FluentValidation.Results;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Runtime;
using HitThatLine.Core.Accounts;
using HitThatLine.Web.Endpoints.Account.Models;
using HitThatLine.Web.Infrastructure.Behaviors;
using Moq;
using NUnit.Framework;
using StructureMap;
using StructureMap.Query;

namespace HitThatLine.Web.Tests.Infrastructure.Behaviors
{
    [TestFixture]
    public class ValidationBehaviorTest
    {
        [Test]
        public void ContinuesIfValid()
        {
            var behavior = buildBehavior();
            behavior.ValidationResult.Errors.Clear();

            behavior.Invoke();

            behavior.MockInsideBehavior.Verify(x => x.Invoke());
            behavior.Request.Verify(x => x.Set(behavior.ValidationResult), Times.Never());
            behavior.Director.Verify(x => x.TransferTo(behavior.Model.TransferToOnFailed), Times.Never());
        }

        [Test]
        public void StopsIfInvalid()
        {
            var behavior = buildBehavior();
            behavior.ValidationResult.Errors.Add(new ValidationFailure("temp", "error"));

            behavior.Invoke();

            behavior.MockInsideBehavior.Verify(x => x.Invoke(), Times.Never());
            behavior.Request.Verify(x => x.Set(behavior.ValidationResult));
            behavior.Director.Verify(x => x.TransferTo(It.Is<object>(p => p.GetType() == behavior.Model.TransferToOnFailed.GetType())));
        }

        private TestableValidationBehavior buildBehavior()
        {
            var request = new Mock<IFubuRequest>();
            var container = new Mock<IContainer>();
            var director = new Mock<IContinuationDirector>();
            var structureMapModel = new Mock<IModel>();
            var validator = new Mock<IValidator<LoginCommand>>();
            var model = new LoginCommand();
            var behavior = new TestableValidationBehavior(request, container.Object, director, model);

            container.SetupGet(x => x.Model).Returns(structureMapModel.Object);
            structureMapModel.Setup(x => x.HasDefaultImplementationFor(typeof(IValidator<LoginCommand>))).Returns(true);
            container.Setup(x => x.GetInstance(typeof(IValidator<LoginCommand>))).Returns(validator.Object);
            request.Setup(x => x.Get<LoginCommand>()).Returns(model);
            validator.Setup(x => x.Validate(model)).Returns(behavior.ValidationResult);

            return behavior;
        }

        private class TestableValidationBehavior : ValidationBehavior<LoginCommand>
        {
            public ValidationResult ValidationResult { get; private set; }
            public Mock<IActionBehavior> MockInsideBehavior { get; private set; }
            public Mock<IFubuRequest> Request { get; private set; }
            public Mock<IContinuationDirector> Director { get; private set; }
            public LoginCommand Model { get; private set; }

            public TestableValidationBehavior(Mock<IFubuRequest> request, IContainer container, Mock<IContinuationDirector> director, LoginCommand model)
                : base(request.Object, container, director.Object)
            {
                ValidationResult = new ValidationResult();
                MockInsideBehavior = new Mock<IActionBehavior>();
                InsideBehavior = MockInsideBehavior.Object;
                Request = request;
                Director = director;
                Model = model;
            }
        }
    }
}