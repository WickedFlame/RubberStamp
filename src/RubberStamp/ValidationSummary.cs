using System.Collections.Generic;
using System.Linq;
using RubberStamp.Internals;

namespace RubberStamp
{
    public class ValidationSummary<T> : IValidationSummary
    {
        private readonly List<IValidationResult> _validationResults;

        public ValidationSummary()
        {
            _validationResults = new List<IValidationResult>();
        }

        public IEnumerable<IValidationResult> ValidationResults => _validationResults;
        
        public bool IsValid => !_validationResults.Any();

        public string ValidationMessage => GetValidationMessage();

        public void AddResult(IValidationResult result)
        {
            _validationResults.Add(result);
        }

        public void Merge(IValidationSummary summary)
        {
            foreach (var result in summary.ValidationResults)
            {
                AddResult(result);
            }
        }

        public string GetErrorMessage()
        {
            var builder = new SummaryMessageBuilder(ValidationResults);
            return builder.BuildMessage(Severity.Error);
        }

        public string GetWarningMessage()
        {
            var builder = new SummaryMessageBuilder(ValidationResults);
            return builder.BuildMessage(Severity.Warning);
        }

        public string GetInfoMessage()
        {
            var builder = new SummaryMessageBuilder(ValidationResults);
            return builder.BuildMessage(Severity.Info);
        }

        private string GetValidationMessage()
        {
            var builder = new SummaryMessageBuilder(ValidationResults);
            return builder.BuildMessage<T>();
        }
    }
}
