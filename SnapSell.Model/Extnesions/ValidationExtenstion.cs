using FluentValidation.Results;

namespace SnapSell.Domain.Extnesions;

public static class ValidationExtenstion
{
    public static Dictionary<string, List<string>> GetErrorsDictionary(this List<ValidationFailure> validationFailures)
    {
        Dictionary<string,List<string>> errors = [];

        validationFailures.ForEach(a =>
        {
            if(errors.ContainsKey(a.PropertyName))
            {
                errors[a.PropertyName].Add(a.ErrorMessage);
            }
            else
            {
                errors.Add(a.PropertyName, [a.ErrorMessage]);
            }
        });

        return errors;
    }
}