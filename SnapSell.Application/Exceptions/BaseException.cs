using System.Collections.ObjectModel;

namespace SnapSell.Application.Exceptions;

public class BaseException(IEnumerable<ValidationError> errors) : Exception
{
    public IEnumerable<ValidationError> Errors { get; } = errors;
}