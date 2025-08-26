namespace velowrench.Utils.Enums;

/// <summary>
/// Defines the validation behavior mode.
/// </summary>
public enum EValidationMode
{
    /// <summary>
    /// Show validation errors progressively as the user interacts with fields.
    /// </summary>
    Progressive,

    /// <summary>
    /// Show all validation errors immediately when validation is triggered.
    /// </summary>
    Immediate,

    /// <summary>
    /// Only show validation errors after the user attempts to submit/calculate.
    /// </summary>
    OnSubmit
}
