using System;
using System.Collections.Generic;

namespace RubberStamp.Rules
{
    public abstract class AbstractValidationRule<T> : IValidationRule<T>
    {
        protected readonly IList<IValidationCondition<T>> ValidationConditions;
        protected string CustomMessage;
        protected string CustomMessageHeader;
        protected Func<T, bool> WhenCondition;

        protected AbstractValidationRule()
        {
            ValidationConditions = new List<IValidationCondition<T>>();
        }

        public IEnumerable<IValidationCondition<T>> Conditions => ValidationConditions;

        public Severity Severity { get; protected set; } = Severity.Error;

        /// <summary>
        /// Set a custom message for the Rule
        /// </summary>
        /// <param name="message">The message that will be set</param>
        /// <returns>A ValidationRule</returns>
        public IValidationRule<T> SetMessage(string message)
        {
            CustomMessage = message;

            return this;
        }

        /// <summary>
        /// Set a custom message header for the Rule
        /// </summary>
        /// <param name="message">The message header that will be set</param>
        /// <returns>A ValidationRule</returns>
        public IValidationRule<T> SetMessageHeader(string message)
        {
            CustomMessageHeader = message;

            return this;
        }

        /// <summary>
        /// Set the severity level for the Rule
        /// </summary>
        /// <param name="severity">The severity level</param>
        /// <returns>A ValidationRule</returns>
        public IValidationRule<T> SetSeverity(Severity severity)
        {
            Severity = severity;

            return this;
        }

        /// <summary>
        /// Add a condition that validates if the Rule is to be checked
        /// </summary>
        /// <param name="condition">The condition to validate</param>
        /// <returns>A ValidationRule</returns>
        public IValidationRule<T> When(Func<T, bool> condition)
        {
            WhenCondition = condition;

            return this;
        }

        public abstract IValidationResult Validate(T instance);
    }
}
