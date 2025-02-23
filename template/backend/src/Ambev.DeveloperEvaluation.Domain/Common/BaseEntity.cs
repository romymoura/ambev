﻿using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Common;

public class BaseEntity : IComparable<BaseEntity>
{
    /// <summary>
    /// created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// last update.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }


    [System.ComponentModel.DataAnnotations.Key]
    public Guid Id { get; set; }

    public Task<IEnumerable<ValidationErrorDetail>> ValidateAsync()
    {
        return Validator.ValidateAsync(this);
    }

    public int CompareTo(BaseEntity? other)
    {
        if (other == null)
        {
            return 1;
        }

        return other!.Id.CompareTo(Id);
    }

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
    public virtual void Activate()
    {
        UpdatedAt = DateTime.UtcNow;
    }
    public virtual void Deactivate()
    {
        UpdatedAt = DateTime.UtcNow;
    }
    public virtual void Suspend()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}