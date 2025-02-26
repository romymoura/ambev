using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application;

public class BaseCommand
{
    public virtual ValidationResultDetail Validate()
    {
        return new ValidationResultDetail();
    }

    public virtual ValidationResultDetail Validate<TValidator, TModel>()
  where TValidator : AbstractValidator<TModel>, new()
  where TModel : class, new()
    {
        var validator = new TValidator();
        var result = validator.Validate(this as TModel);

        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
