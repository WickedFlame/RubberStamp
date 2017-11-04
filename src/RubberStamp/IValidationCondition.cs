using System;
using System.Linq.Expressions;

namespace RubberStamp
{
    public interface IValidationCondition<in T>
    {
        string Message { get; }

        bool IsValid(T property);

        IValidationCondition<T> SetMessage(string message);
    }

    public interface IValidationCondition<T, TProp> : IValidationCondition<T>
    {
        Expression<Func<T, TProp>> Property { get; set; }
    }
}
