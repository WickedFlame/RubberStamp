using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace RubberStamp.Conditions
{
    public class IsValidEmailFormatCondition<T, TProp> : PropertyValidationCondition<T, TProp>
    {
        private const string REG_EX =
            @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public IsValidEmailFormatCondition(Expression<Func<T, TProp>> property)
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

            if (propertyValue == null)
            {
                return false;
            }

            return Regex.IsMatch(propertyValue.ToString(), REG_EX, RegexOptions.IgnoreCase);
        }
    }
}
