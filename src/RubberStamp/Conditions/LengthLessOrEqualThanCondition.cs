using System;
using System.Linq.Expressions;
using RubberStamp.Internals;

namespace RubberStamp.Conditions
{
    public class LengthLessOrEqualThanCondition<T, TProp> : PropertyValidationCondition<T, TProp> 
    {
        private readonly int _value;

        public LengthLessOrEqualThanCondition(int value)
        {
            _value = value;
        }

        public LengthLessOrEqualThanCondition(Expression<Func<T, TProp>> property, int value)
            : this(value)
        {
            Property = property;
        }
        
        public override bool IsValid(T instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var propertyValue = Property.Compile().Invoke(instance);
            return propertyValue == null ||_value.CompareTo(propertyValue.ToString().Length) >= 0;
        }

        protected override string GetDefaultMessage()
        {
            return $"The Property {Property.TryExtractPropertyName()} max length should be {_value}";
        }
    }
}
