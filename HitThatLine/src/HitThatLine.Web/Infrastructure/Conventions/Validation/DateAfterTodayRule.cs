using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using FubuCore.Reflection;
using HitThatLine.Core.Utility;

namespace HitThatLine.Web.Infrastructure.Conventions.Validation
{
    public class DateAfterTodayRule : IValidationRule
    {
        private readonly PropertyChain _propertyChain;
        public DateAfterTodayRule(PropertyChain propertyChain)
        {
            _propertyChain = propertyChain;
        }

        public IEnumerable<ValidationFailure> Validate(ValidationContext context)
        {
            var rawValue = _propertyChain.GetValue(context.InstanceToValidate);
            if (rawValue == null) yield break;

            var value = (DateTime) rawValue;
            var validationFailure = new ValidationFailure(_propertyChain.Name, string.Format("{0} cannot be in the future", context.PropertyChain.BuildPropertyName(_propertyChain.Name).ToPrettyString()));

            if (value > DateTime.Now)
            {
                yield return validationFailure;
            }
        }

        public void ApplyCondition(Func<object, bool> predicate, ApplyConditionTo applyConditionTo = ApplyConditionTo.AllValidators)
        {
            
        }

        public IEnumerable<IPropertyValidator> Validators { get { yield break; } }
        public string RuleSet { get; set; }
    }
}