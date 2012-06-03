using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using FubuCore;
using FubuCore.Reflection;
using HitThatLine.Web.Infrastructure.Conventions.Attributes;

namespace HitThatLine.Web.Infrastructure.Conventions.Validation
{
    public class ConventionBasedValidator<T> : AbstractValidator<T>
    {
        public ConventionBasedValidator()
        {
            validateProperties(typeof(T));
        }

        private void validateProperties(Type type, params PropertyInfo[] precedingProperties)
        {
            foreach (var property in type.GetProperties().Where(x => x.CanRead && x.CanWrite))
            {
                var propertyChain = buildPropertyChain(property, precedingProperties);

                if (property.HasAttribute<RequiredAttribute>())
                {
                    AddRule(new PropertyRequiredRule(propertyChain));
                }
                if (property.HasAttribute<MinLengthAttribute>())
                {
                    AddRule(new MinimumLengthRequiredRule(propertyChain, property.GetAttribute<MinLengthAttribute>().Length));
                }
                if (property.HasAttribute<MaxLengthAttribute>())
                {
                    AddRule(new MaximumLengthRequiredRule(propertyChain, property.GetAttribute<MaxLengthAttribute>().Length));
                }
                if (property.PropertyType.IsTypeOrNullableOf<DateTime>())
                {
                    AddRule(new DateAfterTodayRule(propertyChain));
                }
                if (property.Name.Contains("Email") && property.PropertyType == typeof(string))
                {
                    AddRule(new EmailAddressRule(propertyChain));
                }
                if ((property.Name.Contains("Zip") && property.PropertyType == typeof(string)))
                {
                    AddRule(new MinimumLengthRequiredRule(propertyChain, 5));
                    AddRule(new MaximumLengthRequiredRule(propertyChain, 9));
                }
                if ((property.Name.Contains("Phone") && property.PropertyType == typeof(string)))
                {
                    AddRule(new MinimumLengthRequiredRule(propertyChain, 10));
                    AddRule(new MaximumLengthRequiredRule(propertyChain, 10));
                }
                if ((property.Name.Contains("SocialSecurity") && property.PropertyType == typeof(string)))
                {
                    AddRule(new MinimumLengthRequiredRule(propertyChain, 9));
                    AddRule(new MaximumLengthRequiredRule(propertyChain, 9));
                }
                if (property.PropertyType.IsClass && property.PropertyType.Assembly == typeof(T).Assembly)
                {
                    var allProps = new List<PropertyInfo>(precedingProperties);
                    var propertyList = allProps.ToList();
                    propertyList.Add(property);
                    validateProperties(property.PropertyType, propertyList.ToArray());
                }
            }
        }

        private PropertyChain buildPropertyChain(PropertyInfo property, IEnumerable<PropertyInfo> precedingProperties)
        {
            var valueGetters = precedingProperties.Select(x => new PropertyValueGetter(x)).ToList();
            valueGetters.Add(new PropertyValueGetter(property));
            return new PropertyChain(valueGetters.ToArray());
        }
    }
}