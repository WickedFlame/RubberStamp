using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using RubberStamp.Rules;

namespace RubberStamp
{
    public class Validator<T> : IValidator<T>
    {
        private readonly List<IValidationRule<T>> _rules;

        public Validator()
        {
            _rules = new List<IValidationRule<T>>();
        }

        public IEnumerable<IValidationRule<T>> Rules
        {
            get
            {
                return _rules;
            }
        }

        public IValidationRule<T> AddRule(IValidationRule<T> rule)
        {
            _rules.Add(rule);

            return rule;
        }

        public IValidationRule<T> AddRule(Func<T, bool> condition, string message)
        {
            var rule = new CustomValidationRule<T>(message)
                .AddCondition(new Conditions.CustomEntityCondition<T>(condition));

            AddRule(rule);

            return rule;
        }

        public IValidationRule<T> AddRule<TProp>(Expression<Func<T, TProp>> expression, Func<ValidationConditionBuilder<T, TProp>, IValidationCondition<T>> condition) 
        {
            var rule = new ValidationRule<T, TProp>(expression)
                .AddCondition(condition(new ValidationConditionBuilder<T, TProp>(expression)));

            AddRule(rule);

            return rule;
        }

        public IValidationRule<T> AddRule<TProp>(Expression<Func<T, TProp>> expression, params Func<ValidationConditionBuilder<T, TProp>, IValidationCondition<T>>[] conditions) 
        {
            var rule = new ValidationRule<T, TProp>(expression);
            foreach (var condition in conditions)
            {
                rule.AddCondition(condition(new ValidationConditionBuilder<T, TProp>(expression)));
            }

            AddRule(rule);

            return rule;
        }

        public IValidationRule<T> For<TProp>(Expression<Func<T, TProp>> expression, Func<IValidator<TProp>> validator)
        {
            var rule = new NestedValidationRule<T, TProp>(expression, validator);
            
            AddRule(rule);

            return rule;
        }

        public IValidationRule<T> For<TProp, TVal>(Expression<Func<T, TProp>> expression) where TVal : IValidator<TProp>, new()
        {
            var rule = new NestedValidationRule<T, TProp>(expression, () => new TVal());

            AddRule(rule);

            return rule;
        }

        /// <summary>
        /// Loops over a nested collection and validates all objects of the collection
        /// </summary>
        /// <typeparam name="TProp">The type of object in the collection</typeparam>
        /// <param name="expression">The collection to loop over</param>
        /// <param name="validator">The validator to use</param>
        /// <returns>The validation rule</returns>
        public IValidationRule<T> ForEach<TProp>(Expression<Func<T, IEnumerable<TProp>>> expression, Func<IValidator<TProp>> validator)
        {
            var rule = new EnumeratedValidationRule<T, TProp>(expression, validator);

            AddRule(rule);

            return rule;
        }

        /// <summary>
        /// Loops over a nested collection and validates all objects of the collection
        /// </summary>
        /// <typeparam name="TProp">The type of object in the collection</typeparam>
        /// <typeparam name="TVal">The type of validator to apply</typeparam>
        /// <param name="expression">The collection to loop over</param>
        /// <returns>The validation rule</returns>
        public IValidationRule<T> ForEach<TProp, TVal>(Expression<Func<T, IEnumerable<TProp>>> expression) where TVal : IValidator<TProp>, new()
        {
            var rule = new EnumeratedValidationRule<T, TProp>(expression, () => new TVal());

            AddRule(rule);

            return rule;
        }

        public IValidationSummary Validate(T item)
        {
            var summary = new ValidationSummary<T>();

            foreach (var rule in _rules)
            {
                var result = rule.Validate(item);
                if (result != null)
                {
                    summary.AddResult(result);
                }
            }

            Validate(summary, item);

            return summary;
        }

        protected virtual void Validate(IValidationSummary summary, T item)
        {
            // method that is called for custom validation implemented in inherited object
        }
    }
}
