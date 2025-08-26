using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Validation;

namespace velowrench.Calculations.Validation.Results;

public sealed class ValidationResult
{
    public bool IsValid => this.Errors.Count == 0;
    public IReadOnlyCollection<ValidationError> Errors { get; }
    public IReadOnlyList<string> ErrorMessages => this.Errors.Select(e => e.Message).ToList().AsReadOnly();

    private ValidationResult(IEnumerable<ValidationError>? errors = null)
    {
        if (errors != null)
        {
            this.Errors = new ReadOnlyCollection<ValidationError>(errors.ToList());
        }
        else
        {
            this.Errors = [];
        }
    }

    public static ValidationResult WithSuccess()
    {
        return new ValidationResult();
    }

    public static ValidationResult WithError(string propertyName, string message)
    {
        ValidationError error = new()
        {
            PropertyName = propertyName,
            Message = message
        };
        return new ValidationResult([error]);
    }

    public static ValidationResult WithErrors(IEnumerable<ValidationError> errors)
    {
        return new ValidationResult(errors);
    }

    public static ValidationResult Combine(params ValidationResult[] results)
    {
        IEnumerable<ValidationError> allErrors = results.SelectMany(r => r.Errors);
        return new ValidationResult(allErrors);
    }
}