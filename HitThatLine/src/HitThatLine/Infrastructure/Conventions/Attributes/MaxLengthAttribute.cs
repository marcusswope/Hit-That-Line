using System;

namespace HitThatLine.Infrastructure.Conventions.Attributes
{
    public class MaxLengthAttribute : Attribute
    {
        public int Length { get { return _length; } }

        private readonly int _length;
        public MaxLengthAttribute(int length)
        {
            _length = length;
        }
    }
}