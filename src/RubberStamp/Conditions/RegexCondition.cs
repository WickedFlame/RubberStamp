using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace RubberStamp.Conditions
{
    public class RegexCondition<T, TProp> : PropertyValidationCondition<T, TProp>
    {
        private readonly TProp _value;

        public RegexCondition(TProp value)
        {
            _value = value;
        }

        public RegexCondition(Expression<Func<T, TProp>> property, TProp value)
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

            return Regex.IsMatch(propertyValue.ToString(), _value.ToString(), RegexOptions.IgnoreCase);
        }
    }
}
