using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using RubberStamp.Internals;

namespace RubberStamp.Rules
{
    public class EnumeratedValidationRule<T, TProp> : AbstractValidationRule<T>
    {
        private readonly Expression<Func<T, IEnumerable<TProp>>> _expression;
        private readonly Func<IValidator<TProp>> _validator;

        public EnumeratedValidationRule(Expression<Func<T, IEnumerable<TProp>>> expression, Func<IValidator<TProp>> validator)
        {
            _expression = expression;
            _validator = validator;
        }

        public override IValidationResult Validate(T instance)
        {
            if (WhenCondition != null && !WhenCondition(instance))
            {
                return null;
            }

            var nestedEnumeration = _expression.Compile().Invoke(instance);
            if (nestedEnumeration == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var validator = _validator.Invoke();

            var summaries = new List<IValidationSummary>();

            foreach (var nested in nestedEnumeration)
            {
                var summary = validator.Validate(nested);
                if (!summary.IsValid)
                {
                    summaries.Add(summary);
                }
            }

            if (!summaries.Any())
            {
                return null;
            }

            var results = summaries.SelectMany(s => s.ValidationResults);
            var builder = new SummaryMessageBuilder(results);
            var message = builder.BuildMessage<T>();

            return new ValidationResult(Severity.Error, message);
        }
    }
}
