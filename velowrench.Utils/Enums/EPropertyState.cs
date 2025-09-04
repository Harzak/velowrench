namespace velowrench.Utils.Enums;

/// <summary>
/// Represents the different validation states a property can be in.
/// </summary>
public enum EPropertyState
{
    /// <summary>
    /// The property has not been touched or modified by the user.
    /// </summary>
    Untouched,

    /// <summary>
    /// The user has modified the property value.
    /// </summary>
    Touched,

    /// <summary>
    /// The property has been validated and is valid.
    /// </summary>
    Valid,

    /// <summary>
    /// The property has been validated and has validation errors.
    /// </summary>
    Invalid
}
