using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Runtime;
using HitThatLine.Endpoints.Account.Models;
using HitThatLine.Infrastructure.Validation;
using HitThatLine.Tests.Infrastructure.Conventions.Validation;
using HitThatLine.Tests.Utility;
using Moq;
using NUnit.Framework;
using StructureMap;
using StructureMap.Query;

namespace HitThatLine.Tests.Infrastructure.Behaviors
{
    [TestFixture]
    public class ValidationBehaviorTest
    {
        [Test]
        public void ContinuesIfValid()
        {
            var behavior = TestableValidationBehavior.Build();
            behavior.ValidationResult.Errors.Clear();

            behavior.Invoke();

            behavior.MockInsideBehavior.Verify(x => x.Invoke());
            behavior.Request.Verify(x => x.Set(behavior.ValidationResult), Times.Never());
            behavior.Mapper.Verify(x => x.Map(behavior.Model, behavior.Model.TransferToOnFailed), Times.Never());
            behavior.Director.Verify(x => x.TransferTo(behavior.Model.TransferToOnFailed), Times.Never());
        }

        [Test]
        public void StopsIfInvalid()
        {
            var behavior = TestableValidationBehavior.Build();
            behavior.ValidationResult.Errors.Add(new ValidationFailure("temp", "error"));

            behavior.Invoke();

            behavior.MockInsideBehavior.Verify(x => x.Invoke(), Times.Never());
            behavior.Request.Verify(x => x.Set(behavior.ValidationResult));
            behavior.Mapper.Verify(x => x.Map(behavior.Model, behavior.Model.GetType(), behavior.Model.TransferToOnFailed.GetType()));
            behavior.Director.Verify(x => x.TransferTo(It.Is<object>(p => p.GetType() == behavior.Model.TransferToOnFailed.GetType())));
        }

        private class TestableValidationBehavior : ValidationBehavior<AllConventionsCommand>
        {
            public ValidationResult ValidationResult { get; private set; }
            public Mock<IActionBehavior> MockInsideBehavior { get; private set; }
            public Mock<IFubuRequest> Request { get; private set; }
            public Mock<IContinuationDirector> Director { get; private set; }
            public Mock<IMappingEngine> Mapper { get; private set; }
            public AllConventionsCommand Model { get; private set; }

            public TestableValidationBehavior(Mock<IFubuRequest> request, IContainer container, Mock<IContinuationDirector> director, TestableMappingEngine mapper, AllConventionsCommand model)
                : base(request.Object, container, director.Object, mapper)
            {
                ValidationResult = new ValidationResult();
                MockInsideBehavior = new Mock<IActionBehavior>();
                InsideBehavior = MockInsideBehavior.Object;
                Request = request;
                Director = director;
                Mapper = mapper.Mapper;
                Model = model;
            }

            public static TestableValidationBehavior Build()
            {
                var request = new Mock<IFubuRequest>();
                var container = new Mock<IContainer>();
                var director = new Mock<IContinuationDirector>();
                var structureMapModel = new Mock<IModel>();
                var mapper = new TestableMappingEngine();
                var validator = new Mock<IValidator<AllConventionsCommand>>();
                var model = new AllConventionsCommand();

                container.SetupGet(x => x.Model).Returns(structureMapModel.Object);
                structureMapModel.Setup(x => x.HasDefaultImplementationFor(typeof(IValidator<AllConventionsCommand>))).Returns(true);
                container.Setup(x => x.GetInstance(typeof(IValidator<AllConventionsCommand>))).Returns(validator.Object);
                request.Setup(x => x.Get<AllConventionsCommand>()).Returns(model);
                mapper.Mapper.Setup(x => x.Map(model, model.GetType(), model.TransferToOnFailed.GetType())).Returns(model.TransferToOnFailed);
                mapper.ConfigurationProvider.CreateMap<AllConventionsCommand, AllConventionsCommand>();

                var behavior = new TestableValidationBehavior(request, container.Object, director, mapper, model);
                validator.Setup(x => x.Validate(model)).Returns(behavior.ValidationResult);

                return behavior;
            }
        }
    }
}