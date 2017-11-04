namespace RubberStamp
{
    public interface IValidationResult
    {
        Severity Severity { get; }

        string Message { get; }

        string Property { get; }
    }
}
