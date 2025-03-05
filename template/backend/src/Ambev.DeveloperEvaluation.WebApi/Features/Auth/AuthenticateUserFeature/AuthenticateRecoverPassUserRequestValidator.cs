using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;

/// <summary>
/// Validator for AuthenticateUserRequest
/// </summary>
public class AuthenticateRecoverPassUserRequestValidator : AbstractValidator<AuthenticateUserRequest>
{
    /// <summary>
    /// Initializes validation rules for AuthenticateUserRequest
    /// </summary>
    public AuthenticateRecoverPassUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format");
    }
}
