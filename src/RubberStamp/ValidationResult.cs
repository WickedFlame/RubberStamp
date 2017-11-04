namespace RubberStamp
{
    public class ValidationResult : IValidationResult
    {
        public ValidationResult(Severity severity, string message)
        {
            Severity = severity;
            Message = message;
        }

        public ValidationResult(Severity severity, string message, string property)
            : this(severity, message)
        {
            Property = property;
        }

        public Severity Severity { get; }

        public string Message { get; }

        public string Property { get; }

        public override string ToString()
        {
            return Message;
        }
    }
}
