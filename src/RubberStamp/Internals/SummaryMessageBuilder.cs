using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubberStamp.Internals
{
    internal class SummaryMessageBuilder
    {
        private readonly IEnumerable<IValidationResult> _validationResults;

        public SummaryMessageBuilder(IEnumerable<IValidationResult> validationResults)
        {
            _validationResults = validationResults;
        }

        public string BuildMessage<T>()
        {
            var sb = new StringBuilder();
            var message = BuildMessage(Severity.Error);
            if (!string.IsNullOrEmpty(message))
            {
                sb.AppendLine(message);
            }

            message = BuildMessage(Severity.Warning);
            if (!string.IsNullOrEmpty(message))
            {
                sb.AppendLine(message);
            }

            message = BuildMessage(Severity.Info);
            if (!string.IsNullOrEmpty(message))
            {
                sb.AppendLine(message);
            }

            return sb.ToString().Trim();
        }
        
        public string BuildMessage(Severity severity)
        {
            var sb = new StringBuilder();
            foreach (var result in _validationResults.Where(r => r.Severity == severity))
            {
                sb.AppendLine(result.Message);
            }

            return sb.ToString().Trim();
        }
    }
}
