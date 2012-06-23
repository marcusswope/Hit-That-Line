using AutoMapper;
using FluentValidation;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Runtime;
using HitThatLine.Infrastructure.Conventions.Validation;
using StructureMap;

namespace HitThatLine.Infrastructure.Behaviors
{
    public class ValidationBehavior<T> : IActionBehavior where T : class, IValidatedCommand
    {
        private readonly IFubuRequest _request;
        private readonly IContainer _container;
        private readonly IContinuationDirector _director;
        private readonly IMappingEngine _mapper;

        public IActionBehavior InsideBehavior { get; set; }

        public ValidationBehavior(IFubuRequest request, IContainer container, IContinuationDirector director, IMappingEngine mapper)
        {
            _request = request;
            _container = container;
            _director = director;
            _mapper = mapper;
        }

        public void Invoke()
        {
            IValidator<T> validator;
            if (_container.Model.HasDefaultImplementationFor(typeof(IValidator<>).MakeGenericType(typeof(T))))
            {
                validator = _container.GetInstance(typeof(IValidator<>).MakeGenericType(typeof(T))) as IValidator<T>;
            }
            else
            {
                validator = _container.GetInstance<ConventionBasedValidator<T>>();
            }

            var inputModel = _request.Get<T>();
            var result = validator.Validate(inputModel);
            if (!result.IsValid)
            {
                var transferToModel = inputModel.TransferToOnFailed;
                transferToModel = _mapper.Map(inputModel, transferToModel, inputModel.GetType(), transferToModel.GetType());

                _request.Set(result);
                _director.TransferTo(transferToModel);
                return;
            }

            InsideBehavior.Invoke();
        }

        public void InvokePartial()
        {
            InsideBehavior.InvokePartial();
        }
    }
}