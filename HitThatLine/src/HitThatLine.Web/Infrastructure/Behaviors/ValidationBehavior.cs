using System;
using System.Net;
using FluentValidation;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;
using System.Linq;
using HitThatLine.Web.Infrastructure.Conventions.Validation;
using StructureMap;

namespace HitThatLine.Web.Infrastructure.Behaviors
{
    public class ValidationBehavior<T> : BasicBehavior where T : class
    {
        private readonly IFubuRequest _request;
        private readonly IContainer _container;
        private readonly BehaviorGraph _graph;
        private readonly IContinuationDirector _director;

        public ValidationBehavior(IFubuRequest request, IContainer container, BehaviorGraph graph, IContinuationDirector director)
            : base(PartialBehavior.Ignored)
        {
            _request = request;
            _container = container;
            _graph = graph;
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

            var result = validator.Validate(_request.Get<T>());
            if (result.IsValid) return DoNext.Continue;

            _request.Set(result);
            
            var action = findGetAction();
            _director.TransferToCall(action);
            
            return DoNext.Stop;
        }

        private ActionCall findGetAction()
        {
            var expectedActionInputType = typeof (T).FullName.Replace("View", "Input");
            foreach (var behavior in _graph.Behaviors)
            {
                if (behavior.FirstCall() != null && behavior.FirstCall().InputType() != null)
                {
                    var inputType = behavior.FirstCall().InputType();
                    if (inputType.FullName == expectedActionInputType)
                    {
                        return behavior.FirstCall();
                    }
                }
            }
            throw new InvalidOperationException(string.Format("Expected to find an action with input type: {0} for the corresponding input type: {1}", expectedActionInputType, typeof(T).FullName));
        }
    }
}