using System.Collections.Generic;

namespace RubberStamp
{
    public interface IValidationSummary
    {
        bool IsValid { get; }

        IEnumerable<IValidationResult> ValidationResults { get; }

        string ValidationMessage { get; }

        void AddResult(IValidationResult result);

        void Merge(IValidationSummary summary);

        string GetErrorMessage();

        string GetWarningMessage();

        string GetInfoMessage();
    }
}
