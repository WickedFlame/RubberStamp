using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RubberStamp
{
    public interface IValidator<T>
    {
        IEnumerable<IValidationRule<T>> Rules { get; }

        IValidationRule<T> AddRule(IValidationRule<T> rule);

        IValidationRule<T> AddRule(Func<T, bool> expression, string message);

        IValidationRule<T> AddRule<TProp>(Expression<Func<T, TProp>> expression, Func<ValidationConditionBuilder<T, TProp>, IValidationCondition<T>> condition);

        IValidationRule<T> AddRule<TProp>(Expression<Func<T, TProp>> expression, params Func<ValidationConditionBuilder<T, TProp>, IValidationCondition<T>>[] conditions);

        IValidationSummary Validate(T item);
    }
}
