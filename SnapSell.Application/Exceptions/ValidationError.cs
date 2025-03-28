namespace SnapSell.Application.Exceptions
{
    public sealed record ValidationError(string PropertyName,
        string ErrorMessage);

    public sealed class ValidationException : BaseException
    {
        public ValidationException(IEnumerable<ValidationError> errors):base(errors) { }
    }
}
