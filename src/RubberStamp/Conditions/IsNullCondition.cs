using System;
using System.Linq.Expressions;
using RubberStamp.Internals;

namespace RubberStamp.Conditions
{
    public class IsNullCondition<T, TProp> : PropertyValidationCondition<T, TProp>
    {
        public IsNullCondition()
        {
        }

        public IsNullCondition(Expression<Func<T, TProp>> property)
            : this()
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
            return propertyValue == null;
        }

        protected override string GetDefaultMessage()
        {
            return $"The Property {Property.TryExtractPropertyName()} has to be null";
        }
    }
}
