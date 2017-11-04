using System;
using System.Linq.Expressions;
using RubberStamp.Internals;

namespace RubberStamp.Rules
{
    public class NestedValidationRule<T, TProp> : AbstractValidationRule<T>
    {
        private readonly Expression<Func<T, TProp>> _expression;
        private readonly Func<IValidator<TProp>> _validator;

        public NestedValidationRule(Expression<Func<T, TProp>> expression, Func<IValidator<TProp>> validator)
        {
            _expression = expression;
            _validator = validator;
        }

        public override IValidationResult Validate(T instance)
        {
            var nested = _expression.Compile().Invoke(instance);

            if (WhenCondition != null && !WhenCondition(instance))
            {
                return null;
            }

            var validator = _validator.Invoke();
            var summary = validator.Validate(nested);
            if (summary.IsValid)
            {
                return null;
            }

            var builder = new SummaryMessageBuilder(summary.ValidationResults);
            var message = builder.BuildMessage<T>();

            return new ValidationResult(Severity.Error, message);
        }
    }
}
