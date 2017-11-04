using System;
using System.Linq.Expressions;
using RubberStamp.Internals;

namespace RubberStamp.Conditions
{
    public class CustomPropertyCondition<T, TProp> : PropertyValidationCondition<T, TProp>
    {
        private readonly Func<T, TProp, bool> _condition;

        public CustomPropertyCondition(Func<T, TProp, bool> condition)
        {
            _condition = condition;
        }

        public CustomPropertyCondition(Expression<Func<T, TProp>> property, Func<T, TProp, bool> condition)
            : this(condition)
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
            return _condition(instance, propertyValue);
        }

        protected override string GetDefaultMessage()
        {
            return $"The Property {Property.TryExtractPropertyName()} caused a validation error";
        }
    }
}
