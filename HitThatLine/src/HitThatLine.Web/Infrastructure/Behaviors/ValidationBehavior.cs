using FluentValidation;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Runtime;
using HitThatLine.Web.Infrastructure.Conventions.Validation;
using StructureMap;

namespace HitThatLine.Web.Infrastructure.Behaviors
{
    public class ValidationBehavior<T> : BasicBehavior where T : class, IValidatedCommand
    {
        private readonly IFubuRequest _request;
        private readonly IContainer _container;
        private readonly IContinuationDirector _director;

        public ValidationBehavior(IFubuRequest request, IContainer container, IContinuationDirector director)
            : base(PartialBehavior.Ignored)
        {
            _request = request;
            _container = container;
            _director = director;
        }

        protected override DoNext performInvoke()
        {
            IValidator<T> validator;
            if (_container.Model.HasDefaultImplementationFor(typeof (IValidator<>).MakeGenericType(typeof (T))))
            {
                validator = _container.GetInstance(typeof (IValidator<>).MakeGenericType(typeof (T))) as IValidator<T>;
            }
            else
            {
                validator = _container.GetInstance<ConventionBasedValidator<T>>();
            }

            var model = _request.Get<T>();
            var result = validator.Validate(model);
            if (result.IsValid) return DoNext.Continue;

            _request.Set(result);
            _director.TransferTo(model.TransferOnFailed);
            
            return DoNext.Stop;
        }
    }
}