using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using FubuCore.Reflection;
using HitThatLine.Utility;

namespace HitThatLine.Infrastructure.Conventions.Validation
{
    public class MinimumLengthRequiredRule : IValidationRule
    {
        private readonly PropertyChain _propertyChain;
        private readonly int _length;

        public MinimumLengthRequiredRule(PropertyChain propertyChain, int length)
        {
            _propertyChain = propertyChain;
            _length = length;
        }

        public IEnumerable<ValidationFailure> Validate(ValidationContext context)
        {
            var rawValue = _propertyChain.GetValue(context.InstanceToValidate);
            var validationFailure = new ValidationFailure(_propertyChain.Name, string.Format("{0} must be at least {1} characters", context.PropertyChain.BuildPropertyName(_propertyChain.Name).ToPrettyString(), _length));
            if (rawValue != null && rawValue.ToString().Length < _length)
            {
                yield return validationFailure;
            }
        }

        public void ApplyCondition(Func<object, bool> predicate, ApplyConditionTo applyConditionTo = ApplyConditionTo.AllValidators)
        {
            
        }

        public IEnumerable<IPropertyValidator> Validators
        {
            get { yield break; }
        }

        public string RuleSet { get; set; }
    }
}