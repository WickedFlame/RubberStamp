using System;
using System.Linq.Expressions;

namespace RubberStamp.Conditions
{
    public abstract class PropertyValidationCondition<T, TProp> : IValidationCondition<T, TProp>
    {
        private string _message;

        public Expression<Func<T, TProp>> Property { get; set; }

        public string Message
        {
            get
            {
                if (string.IsNullOrEmpty(_message))
                {
                    return GetDefaultMessage();
                }

                return _message;
            }

            protected set
            {
                _message = value;
            }
        }
        
        public abstract bool IsValid(T instance);

        public IValidationCondition<T> SetMessage(string message)
        {
            Message = message;

            return this;
        }

        protected virtual string GetDefaultMessage()
        {
            return string.Empty;
        }
    }
}
