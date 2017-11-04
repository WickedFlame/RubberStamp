using System;
using System.Linq.Expressions;
using System.Text;
using RubberStamp.Internals;

namespace RubberStamp.Rules
{
    public class ValidationRule<T, TPrp> : AbstractValidationRule<T>, IValidationRule<T, TPrp>
    {
        private readonly Expression<Func<T, TPrp>> _property;

        public ValidationRule(Expression<Func<T, TPrp>> property)
        {
            _property = property;
        }

        /// <summary>
        /// Adds a Condition to the rule. If the condition needs a Property to be validated, the property has to be added to the condition. This will not be done automaticaly.
        /// </summary>
        /// <param name="condition">The condition that will be validated</param>
        /// <returns>A ValidationRule</returns>
        public virtual IValidationRule<T> AddCondition(IValidationCondition<T> condition)
        {
            ValidationConditions.Add(condition);

            return this;
        }

        /// <summary>
        /// Adds a Property based Condition to the rule. The Property is added to the Condition if not provided by the condition.
        /// </summary>
        /// <param name="condition">The condition that will be validated</param>
        /// <returns>A Property bsed ValidationRule</returns>
        public virtual IValidationRule<T, TPrp> AddCondition(IValidationCondition<T, TPrp> condition)
        {
            if (condition.Property == null)
            {
                condition.Property = _property;
            }

            ValidationConditions.Add(condition);

            return this;
        }

        /// <summary>
        /// Set a custom message for the Rule
        /// </summary>
        /// <param name="message">The message that will be set</param>
        /// <returns>A Property based ValidationRule</returns>
        public new IValidationRule<T, TPrp> SetMessage(string message)
        {
            CustomMessage = message;

            return this;
        }

        /// <summary>
        /// Set a custom message header for the Rule
        /// </summary>
        /// <param name="message">The message header that will be set</param>
        /// <returns>A Property based ValidationRule</returns>
        public new IValidationRule<T, TPrp> SetMessageHeader(string message)
        {
            CustomMessageHeader = message;

            return this;
        }

        /// <summary>
        /// Set the severity level for the Rule
        /// </summary>
        /// <param name="severity">The severity level</param>
        /// <returns>A Property based ValidationRule</returns>
        public new IValidationRule<T, TPrp> SetSeverity(Severity severity)
        {
            Severity = severity;

            return this;
        }

        /// <summary>
        /// Add a condition that validates if the Rule is to be checked
        /// </summary>
        /// <param name="condition">The condition to validate</param>
        /// <returns>A ValidationRule</returns>
        public new IValidationRule<T, TPrp> When(Func<T, bool> condition)
        {
            WhenCondition = condition;

            return this;
        }

        public override IValidationResult Validate(T instance)
        {
            StringBuilder message = null;

            if (WhenCondition != null && !WhenCondition(instance))
            {
                return null;
            }

            foreach (var condition in Conditions)
            {
                if (condition.IsValid(instance))
                {
                    continue;
                }

                if (message == null)
                {
                    message = new StringBuilder();
                }
                else
                {
                    message.AppendLine();
                }

                message.Append(condition.Message);
            }

            if (message == null)
            {
                return null;
            }

            return new ValidationResult(Severity, string.Concat(CustomMessageHeader, CustomMessage ?? message.ToString()), _property.TryExtractPropertyName());
        }
    }
}
