﻿using System;
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

        /// <summary>
        /// Gets a collection of all rules associated with the validator
        /// </summary>
        public IEnumerable<IValidationRule<T>> Rules
        {
            get
            {
                return _rules;
            }
        }

        /// <summary>
        /// Add a new rule to the validator
        /// </summary>
        /// <param name="rule">The new rule</param>
        /// <returns>The validator</returns>
        public Validator<T> AddRule(IValidationRule<T> rule)
        {
            _rules.Add(rule);

            return this;
        }

        /// <summary>
        /// Add a new rule to the validator
        /// </summary>
        /// <param name="condition">The condition to check</param>
        /// <param name="message">The message that is returned in case of an error</param>
        /// <returns>The validator</returns>
        public Validator<T> AddRule(Func<T, bool> condition, string message)
        {
            var rule = new CustomValidationRule<T>(message)
                .AddCondition(new Conditions.CustomEntityCondition<T>(condition));

            AddRule(rule);

            return this;
        }

        /// <summary>
        /// Add a new rule to the validator
        /// </summary>
        /// <typeparam name="TProp">The property to check</typeparam>
        /// <param name="expression">The property to check</param>
        /// <param name="condition">The condition to check</param>
        /// <param name="extensions">The additional properties of the rule</param>
        /// <returns>The validator</returns>
        public Validator<T> AddRule<TProp>(Expression<Func<T, TProp>> expression, Func<ValidationConditionBuilder<T, TProp>, IValidationCondition<T>> condition, params Expression<Action<IValidationRule<T>>>[] extensions) 
        {
            var rule = new ValidationRule<T, TProp>(expression);

            var builder = new ValidationConditionBuilder<T, TProp>(expression, rule);
            var validation = condition(builder);

            rule.AddCondition(validation);

            AddRule(rule);

            foreach (var extension in extensions)
            {
                extension.Compile().Invoke(rule);
            }

            return this;
        }

        /// <summary>
        /// Add a set of new rules to the validator
        /// </summary>
        /// <typeparam name="TProp">The property to check</typeparam>
        /// <param name="expression">The property to check</param>
        /// <param name="conditions">The conditions to check</param>
        /// <param name="extensions">The additional properties of the rule</param>
        /// <returns>The validator</returns>
        public Validator<T> AddRule<TProp>(Expression<Func<T, TProp>> expression, Func<ValidationConditionBuilder<T, TProp>, IEnumerable<IValidationCondition<T>>> conditions, params Expression<Action<IValidationRule<T>>>[] extensions)
        {
            var rule = new ValidationRule<T, TProp>(expression);

            var builder = new ValidationConditionBuilder<T, TProp>(expression, rule);
            var validations = conditions(builder);

            foreach (var validation in validations)
            {
                rule.AddCondition(validation);
            }

            AddRule(rule);

            foreach (var extension in extensions)
            {
                extension.Compile().Invoke(rule);
            }

            return this;
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

        /// <summary>
        /// Validates the object with the rules associated
        /// </summary>
        /// <param name="item">The object to validate</param>
        /// <returns>The validationsummary</returns>
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
