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
    public class PropertyRequiredRule : IValidationRule
    {
        private readonly PropertyChain _property;
        public PropertyRequiredRule(PropertyChain property)
        {
            _property = property;
        }

        public IEnumerable<ValidationFailure> Validate(ValidationContext context)
        {
            var rawValue = _property.GetValue(context.InstanceToValidate);
            var validationFailure = new ValidationFailure(_property.Name, "{0} is required".ToFormat(context.PropertyChain.BuildPropertyName(_property.Name).ToPrettyString()));

            if (_property.PropertyType == typeof(string) && string.IsNullOrWhiteSpace((string)rawValue))
            {
                yield return validationFailure;
            }
            else if (_property.PropertyType == typeof(DateTime) && (DateTime)rawValue == default(DateTime))
            {
                yield return validationFailure;
            }
            else if (_property.PropertyType == typeof(DateTime?) && !((DateTime?)rawValue).HasValue)
            {
                yield return validationFailure;
            }
            else if (_property.PropertyType == typeof(int?) && !((int?)rawValue).HasValue)
            {
                yield return validationFailure;
            }
            else if (_property.PropertyType == typeof(int) && (int)rawValue == default(int))
            {
                yield return validationFailure;
            }
            else if (Nullable.GetUnderlyingType(_property.PropertyType) != null && rawValue == null)
            {
                yield return validationFailure;
            }
            else if (_property.PropertyType.IsClass && rawValue == null)
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