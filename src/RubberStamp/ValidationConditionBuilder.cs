using System;
using System.Linq.Expressions;
using RubberStamp.Conditions;

namespace RubberStamp
{
    public class ValidationConditionBuilder<T, TProp>
    {
        private readonly Expression<Func<T, TProp>> _property;

        internal ValidationConditionBuilder(Expression<Func<T, TProp>> property)
        {
            _property = property;
        }

        public IValidationCondition<T> Condition(Func<T, TProp, bool> condition)
        {
            return new CustomPropertyCondition<T, TProp>(_property, condition);
        }

        public IValidationCondition<T> IsNotNull()
        {
            return new IsNotNullCondition<T, TProp>(_property);
        }

        public IValidationCondition<T> IsNull()
        {
            return new IsNullCondition<T, TProp>(_property);
        }

        public IValidationCondition<T> GreaterThan(TProp value) 
        {
            return new GreaterThanCondition<T, TProp>(_property, value);
        }

        public IValidationCondition<T> GreaterOrEqualThan(TProp value) 
        {
            return new GreaterOrEqualThanCondition<T, TProp>(_property, value);
        }

        public IValidationCondition<T> LessThan(TProp value)
        {
            return new LessThanCondition<T, TProp>(_property, value);
        }

        public IValidationCondition<T> LessOrEqualThan(TProp value) 
        {
            return new LessOrEqualThanCondition<T, TProp>(_property, value);
        }

        public IValidationCondition<T> LengthLessOrEqualThan(int value)
        {
            return new LengthLessOrEqualThanCondition<T, TProp>(_property, value);
        }

        public IValidationCondition<T> IsEqual(TProp value) 
        {
            return new IsEqualCondition<T, TProp>(_property, value);
        }

        public IValidationCondition<T> IsNotEqual(TProp value) 
        {
            return new IsNotEqualCondition<T, TProp>(_property, value);
        }

        public IValidationCondition<T> IsValidEmailFormat()
        {
            return new IsValidEmailFormatCondition<T, TProp>(_property);
        }

        public IValidationCondition<T> Regex(TProp value)
        {
            return new RegexCondition<T, TProp>(_property, value);
        }
    }
}