using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RubberStamp
{
    public interface IValidator<T>
    {
        /// <summary>
        /// Gets a collection of all rules associated with the validator
        /// </summary>
        IEnumerable<IValidationRule<T>> Rules { get; }

        /// <summary>
        /// Add a new rule to the validator
        /// </summary>
        /// <param name="rule">The new rule</param>
        /// <returns>The validator</returns>
        Validator<T> AddRule(IValidationRule<T> rule);

        /// <summary>
        /// Add a new rule to the validator
        /// </summary>
        /// <param name="condition">The condition to check</param>
        /// <param name="message">The message that is returned in case of an error</param>
        /// <returns>The validator</returns>
        Validator<T> AddRule(Func<T, bool> condition, string message);

        /// <summary>
        /// Add a new rule to the validator
        /// </summary>
        /// <typeparam name="TProp">The property to check</typeparam>
        /// <param name="expression">The property to check</param>
        /// <param name="condition">The condition to check</param>
        /// <param name="extensions">The additional properties of the rule</param>
        /// <returns>The validator</returns>
        Validator<T> AddRule<TProp>(Expression<Func<T, TProp>> expression, Func<ValidationConditionBuilder<T, TProp>, IValidationCondition<T>> condition, params Expression<Action<IValidationRule<T>>>[] extensions);

        /// <summary>
        /// Add a set of new rules to the validator
        /// </summary>
        /// <typeparam name="TProp">The property to check</typeparam>
        /// <param name="expression">The property to check</param>
        /// <param name="conditions">The conditions to check</param>
        /// <param name="extensions">The additional properties of the rule</param>
        /// <returns>The validator</returns>
        Validator<T> AddRule<TProp>(Expression<Func<T, TProp>> expression, Func<ValidationConditionBuilder<T, TProp>, IEnumerable<IValidationCondition<T>>> conditions, params Expression<Action<IValidationRule<T>>>[] extensions);
        
        IValidationSummary Validate(T item);
    }
}
