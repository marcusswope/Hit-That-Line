using System;

namespace HitThatLine.Infrastructure.Validation.Attributes
{
    public class MinLengthAttribute : Attribute
    {
        public int Length { get { return _length; } }

        private readonly int _length;
        public MinLengthAttribute(int length)
        {
            _length = length;
        }
    }
}