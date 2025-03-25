namespace SnapSell.Application.Exceptions
{
    public class BaseException: Exception
    {
        public BaseException(IEnumerable<ValidationError> errors) => Errors = errors;
        public IEnumerable<ValidationError> Errors { get; }
    }
}
