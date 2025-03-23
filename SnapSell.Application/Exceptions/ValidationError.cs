﻿namespace SnapSell.Application.Exceptions
{
    public sealed record ValidationError(string PropertyName,
        string ErrorMessage);

    public sealed class ValidationException : Exception
    {
        public ValidationException(IEnumerable<ValidationError> errors) => Errors = errors;
        public IEnumerable<ValidationError> Errors { get; }
    }
}
