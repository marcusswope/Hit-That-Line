using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using FubuCore.Reflection;
using HitThatLine.Utility;
using FubuCore;

namespace HitThatLine.Infrastructure.Validation
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
            var validationFailure = new ValidationFailure(_propertyChain.Name, "{0} cannot be in the future".ToFormat(context.PropertyChain.BuildPropertyName(_propertyChain.Name).ToPrettyString()));

            if (value > DateTime.UtcNow)
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