using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core.Continuations;
using NUnit.Framework;

namespace HitThatLine.Web.Tests.Utility
{
    public static class GeneralTestingExtensions
    {
        public static void ShouldBeFalse(this bool condition)
        {
            Assert.False(condition);
        }

        public static void ShouldBeFalse(this bool condition, string userMessage)
        {
            Assert.False(condition, userMessage);
        }

        public static void ShouldBeTrue(this bool condition)
        {
            Assert.True(condition);
        }

        public static void ShouldBeTrue(this bool condition, string userMessage)
        {
            Assert.True(condition, userMessage);
        }

        public static void ShouldBeEmpty(this IEnumerable collection)
        {
            Assert.AreEqual(0, collection.Cast<object>().Count());
        }

        public static void ShouldContain<T>(this IEnumerable<T> collection, T expected)
        {
            Assert.IsTrue(collection.Contains(expected));
        }

        public static void ShouldNotBeEmpty(this IEnumerable collection)
        {
            Assert.Greater(collection.Cast<object>().Count(), 0);
        }

        public static void ShouldNotContain<T>(this IEnumerable<T> collection, T expected)
        {
            Assert.IsFalse(collection.Contains(expected));
        }

        public static void ShouldBeWithinOneSecondFromNow(this DateTime actual)
        {
            ShouldBeWithinOneSecondFromNow((DateTime?)actual);
        }

        public static void ShouldBeWithinOneSecondFromNow(this DateTime? actual)
        {
            if (!actual.HasValue)
                Assert.True(false, "The value is null; therefore not within one second from now.");

            DateTime dateValue = actual.Value;
            DateTime now = (dateValue.Kind == DateTimeKind.Utc) ? DateTime.UtcNow : DateTime.Now;

            ShouldBeWithinOneSecondFrom(actual, now);
        }

        public static void ShouldBeWithinOneSecondFrom(this DateTime actual, DateTime expected)
        {
            ShouldBeWithinOneSecondFrom((DateTime?)actual, expected);
        }

        public static void ShouldBeWithinOneSecondFrom(this DateTime? actual, DateTime expected)
        {
            if (!actual.HasValue)
                Assert.True(false, String.Format("The actual value is null; therefore not within one second from {0}.", expected));

            DateTime dateValue = actual.Value;
            TimeSpan oneSecond = new TimeSpan(0, 0, 1);

            DateTime lower = expected.Subtract(oneSecond);
            DateTime upper = expected.Add(oneSecond);

            Assert.Greater(dateValue, lower);
            Assert.Less(dateValue, upper);
        }

        public static void ShouldBeNull(this object @object)
        {
            Assert.Null(@object);
        }

        public static void ShouldBeSameAs(this object actual, object expected)
        {
            Assert.ReferenceEquals(expected, actual);
        }

        public static ShouldBeOfTypeAnd<T> ShouldBeOfType<T>(this object @object)
        {
            Assert.IsInstanceOf<T>(@object);
            return new ShouldBeOfTypeAnd<T>((T)@object);
        }

        public static void ShouldBeOfType(this object @object, Type expectedType)
        {
            Assert.IsInstanceOf(expectedType, @object);
        }

        public static T ShouldBeAssignableFrom<T>(this object @object)
        {
            Assert.IsAssignableFrom<T>(@object);
            return (T)@object;
        }

        public static void ShouldBeAssignableFrom(this object @object, Type expectedType)
        {
            Assert.IsAssignableFrom(expectedType, @object);
        }

        public static void ShouldEqual<T>(this T actual, T expected)
        {
            Assert.AreEqual(expected, actual);
        }

        public static void ShouldNotBeNull(this object @object)
        {
            Assert.NotNull(@object);
        }

        public static void ShouldNotBeSameAs(this object actual, object expected)
        {
            Assert.IsTrue(ReferenceEquals(actual, expected));
        }

        public static void ShouldNotBeType<T>(this object @object)
        {
            Assert.IsNotInstanceOf<T>(@object);
        }

        public static void ShouldNotBeType(this object @object, Type expectedType)
        {
            Assert.IsNotInstanceOf(expectedType, @object);
        }

        public static void ShouldNotEqual<T>(this T actual, T expected)
        {
            Assert.AreNotEqual(expected, actual);
        }

        public static void ShouldContain(this string actualString, string expectedSubString)
        {
            Assert.IsTrue(actualString.Contains(expectedSubString));
        }

        public static void ShouldNotContain(this string actualString, string expectedSubString)
        {
            Assert.IsFalse(actualString.Contains(expectedSubString));
        }

        public static void ShouldHavePDF(this HttpResponseBase response, byte[] pdf)
        {
            response.ContentType.ShouldEqual("application/pdf");
            response.Headers["content-disposition"].ShouldEqual("inline;filename=ProductDetails.pdf");
        }

        public static void AssertWasRedirectedTo<T>(this FubuContinuation continuation) 
            where T : class
        {
            continuation.AssertWasRedirectedTo<T>(x => x != null);
        }

        public class ShouldBeOfTypeAnd<T>
        {
            private readonly T _object;

            public ShouldBeOfTypeAnd(T @object)
            {
                _object = @object;
            }

            public T And()
            {
                return _object;
            }
        }
    }
}