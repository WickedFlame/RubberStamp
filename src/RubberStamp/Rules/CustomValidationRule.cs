using System.Text;

namespace RubberStamp.Rules
{
    public class CustomValidationRule<T> : AbstractValidationRule<T>
    {
        public CustomValidationRule()
            : this(null)
        {
        }

        public CustomValidationRule(string message)
        {
            CustomMessage = message;
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

        public override IValidationResult Validate(T instance)
        {
            if (WhenCondition != null && WhenCondition(instance))
            {
                return null;
            }

            var error = false;
            StringBuilder message = null;

            foreach (var condition in Conditions)
            {
                if (condition.IsValid(instance))
                {
                    continue;
                }

                error = true;

                if (message == null)
                {
                    message = new StringBuilder();
                }
                else
                {
                    message.Append(" and ");
                }

                message.Append($"{condition.Message}");
            }

            if (!error)
            {
                return null;
            }

            return new ValidationResult(Severity, CustomMessage ?? message.ToString());
        }
    }
}
