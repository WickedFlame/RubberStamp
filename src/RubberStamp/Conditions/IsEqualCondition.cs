﻿using System;
using System.Linq.Expressions;
using RubberStamp.Internals;

namespace RubberStamp.Conditions
{
    public class IsEqualCondition<T, TProp> : PropertyValidationCondition<T, TProp> 
    {
        private readonly TProp _value;

        public IsEqualCondition(TProp value)
        {
            _value = value;
        }

        public IsEqualCondition(Expression<Func<T, TProp>> property, TProp value)
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

            var comparable = _value as IComparable;
            if (comparable != null)
            {
                return comparable.CompareTo(propertyValue) == 0;
            }
            
            Message = $"The Property {Property.TryExtractPropertyName()} could not be compared because it is not a IComparable type";
            return false;
        }

        protected override string GetDefaultMessage()
        {
            return $"The Property {Property.TryExtractPropertyName()} has to be equal to {_value}";
        }
    }
}
