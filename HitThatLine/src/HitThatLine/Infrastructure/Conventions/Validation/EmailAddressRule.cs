using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using FubuCore.Reflection;
using HitThatLine.Utility;

namespace HitThatLine.Infrastructure.Conventions.Validation
{
    public class EmailAddressRule : IValidationRule
    {
        private readonly PropertyChain _propertyChain;
        private readonly EmailValidator _validator;

        public EmailAddressRule(PropertyChain propertyChain)
        {
            _propertyChain = propertyChain;
            _validator = new EmailValidator();
        }

        public IEnumerable<ValidationFailure> Validate(ValidationContext context)
        {
            var rawValue = _propertyChain.GetValue(context.InstanceToValidate);
            if (rawValue == null) yield break;

            var regex = new Regex(_validator.Expression);
            if (!regex.IsMatch(rawValue.ToString()))
            {
                yield return new ValidationFailure(_propertyChain.Name, string.Format("{0} is not a valid email address", context.PropertyChain.BuildPropertyName(_propertyChain.Name).ToPrettyString()));
            }
        }

        public void ApplyCondition(Func<object, bool> predicate, ApplyConditionTo applyConditionTo = ApplyConditionTo.AllValidators)
        {
            
        }

        public IEnumerable<IPropertyValidator> Validators { get { yield break; } }
        public string RuleSet { get; set; }
    }
}