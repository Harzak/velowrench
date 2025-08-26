using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.EventArg;
using velowrench.Utils.Enums;

namespace velowrench.Core.Interfaces;

/// <summary>
/// Manages the validation state for individual properties in an input object.
/// </summary>
/// <typeparam name="TInput">The type of input object being validated.</typeparam>
public interface IPropertyStateManager<TInput>
{
    /// <summary>
    /// Gets the validation mode for determining when to show errors.
    /// </summary>
    EValidationMode ValidationMode { get; set; }

    /// <summary>
    /// Occurs when the validation state of a property changes.
    /// </summary>
    event EventHandler<EPropertyStateChangedEventArgs>? StateChanged;

    /// <summary>
    /// Gets the current validation state of the specified property.
    /// </summary>
    EPropertyState GetPropertyState(string propertyName);

    /// <summary>
    /// Marks a property as touched (user has interacted with it).
    /// </summary>
    void MarkPropertyTouched(string propertyName);

    /// <summary>
    /// Marks a property as dirty (user has modified its value).
    /// </summary>
    void MarkPropertyDirty(string propertyName);

    /// <summary>
    /// Marks a property as validating.
    /// </summary>
    void MarkPropertyValidating(string propertyName);

    /// <summary>
    /// Marks a property as valid after successful validation.
    /// </summary>
    void MarkPropertyValid(string propertyName);

    /// <summary>
    /// Marks a property as invalid after failed validation.
    /// </summary>
    void MarkPropertyInvalid(string propertyName);

    /// <summary>
    /// Determines whether validation errors should be shown for the specified property
    /// based on its current state and the validation mode.
    /// </summary>
    bool ShouldShowErrors(string propertyName);

    /// <summary>
    /// Resets the validation state of all properties to pristine.
    /// </summary>
    void Reset();
}
