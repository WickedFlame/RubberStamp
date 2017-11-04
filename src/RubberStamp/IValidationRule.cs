using System;
using System.Collections.Generic;

namespace RubberStamp
{
    public interface IValidationRule<T>
    {
        IEnumerable<IValidationCondition<T>> Conditions { get; }

        Severity Severity { get; }
        
        IValidationResult Validate(T item);

        /// <summary>
        /// Set a custom message for the Rule
        /// </summary>
        /// <param name="message">The message that will be set</param>
        /// <returns>A ValidationRule</returns>
        IValidationRule<T> SetMessage(string message);

        /// <summary>
        /// Set a custom message header for the Rule
        /// </summary>
        /// <param name="message">The message header that will be set</param>
        /// <returns>A ValidationRule</returns>
        IValidationRule<T> SetMessageHeader(string message);

        /// <summary>
        /// Set the severity level for the Rule
        /// </summary>
        /// <param name="severity">The severity level</param>
        /// <returns>A ValidationRule</returns>
        IValidationRule<T> SetSeverity(Severity severity);

        /// <summary>
        /// Add a condition that validates if the Rule is to be checked
        /// </summary>
        /// <param name="condition">The condition to validate</param>
        /// <returns>A ValidationRule</returns>
        IValidationRule<T> When(Func<T, bool> condition);
    }
    
    public interface IValidationRule<T, TProp> : IValidationRule<T>
    {
        /// <summary>
        /// Adds a Property based Condition to the rule. The Property is added to the Condition if not provided by the condition.
        /// </summary>
        /// <param name="condition">The condition that will be validated</param>
        /// <returns>A Property bsed ValidationRule</returns>
        IValidationRule<T, TProp> AddCondition(IValidationCondition<T, TProp> condition);

        /// <summary>
        /// Set a custom message for the Rule
        /// </summary>
        /// <param name="message">The message that will be set</param>
        /// <returns>A Property based ValidationRule</returns>
        new IValidationRule<T, TProp> SetMessage(string message);

        /// <summary>
        /// Set a custom message header for the Rule
        /// </summary>
        /// <param name="message">The message header that will be set</param>
        /// <returns>A Property based ValidationRule</returns>
        new IValidationRule<T, TProp> SetMessageHeader(string message);

        /// <summary>
        /// Set the severity level for the Rule
        /// </summary>
        /// <param name="severity">The severity level</param>
        /// <returns>A Property based ValidationRule</returns>
        new IValidationRule<T, TProp> SetSeverity(Severity severity);

        /// <summary>
        /// Add a condition that validates if the Rule is to be checked
        /// </summary>
        /// <param name="condition">The condition to validate</param>
        /// <returns>A ValidationRule</returns>
        new IValidationRule<T, TProp> When(Func<T, bool> condition);
    }
}
