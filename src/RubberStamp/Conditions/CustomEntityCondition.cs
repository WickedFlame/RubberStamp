using System;

namespace RubberStamp.Conditions
{
    public class CustomEntityCondition<T> : IValidationCondition<T>
    {
        private readonly Func<T, bool> _condition;

        public CustomEntityCondition(Func<T, bool> condition)
        {
            _condition = condition;

            Message = $"The Validation failed for the Entity {typeof(T).Name}";
        }

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public string Message { get; private set; }
        
        public bool IsValid(T instance)
        {
            return _condition(instance);
        }

        public IValidationCondition<T> SetMessage(string message)
        {
            Message = message;

            return this;
        }
    }
}
