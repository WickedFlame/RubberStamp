using System;
using System.Collections;
using System.Collections.Generic;

namespace RubberStamp
{
    public class ValidationContainer<T> : IEnumerable<ValidationItem<T>>
    {
        private readonly List<ValidationItem<T>> _validators;

        public ValidationContainer()
        {
            _validators = new List<ValidationItem<T>>();
        }

        public ValidationItem<T> Add(Validator<T> validator)
        {
            if (validator == null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            var item = new ValidationItem<T>(validator);
            _validators.Add(item);

            return item;
        }

        public IValidationSummary Validate(T item)
        {
            var summary = new ValidationSummary<T>();

            foreach (var validator in _validators)
            {
                var result = validator.Validate(item);
                if (!result.IsValid)
                {
                    foreach (var res in result.ValidationResults)
                    {
                        summary.AddResult(res);
                    }
                }
            }

            return summary;
        }

        public IEnumerator<ValidationItem<T>> GetEnumerator()
        {
            return _validators.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ValidationItem<T>
    {
        private readonly Validator<T> _validator;

        public ValidationItem(Validator<T> validator)
        {
            _validator = validator;
        }

        public IValidationSummary Validate(T item)
        {
            return _validator.Validate(item);
        }

        public ValidationItem<T> ExpectedResult(Func<object> reslut)
        {
            throw new NotImplementedException();
        }
    }
}
