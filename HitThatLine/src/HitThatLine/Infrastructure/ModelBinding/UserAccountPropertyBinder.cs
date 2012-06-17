﻿using System.Reflection;
using FubuCore.Binding;
using FubuCore;
using HitThatLine.Domain.Accounts;
using HitThatLine.Services;
using Raven.Client;
using System.Linq;

namespace HitThatLine.Infrastructure.ModelBinding
{
    public class UserAccountPropertyBinder : IPropertyBinder
    {
        public bool Matches(PropertyInfo property)
        {
            return property.PropertyType.CanBeCastTo<UserAccount>();
        }

        public void Bind(PropertyInfo property, IBindingContext context)
        {
            UserAccount user = null;
            if (context.Service<ICookieStorage>().Contains(UserAccount.LoginCookieName))
            {
                var userId = context.Service<ICookieStorage>().Get(UserAccount.LoginCookieName);
                user = context.Service<IDocumentSession>().Load<UserAccount>(userId);
            }
            else
            {
                var username = context.Data.ValueAs<string>("Username");
                if (username != null)
                {
                    user = context.Service<IDocumentSession>().Query<UserAccount>().FirstOrDefault(x => x.Username == username);
                }
            }

            property.SetValue(context.Object, user, null);
        }
    }
}