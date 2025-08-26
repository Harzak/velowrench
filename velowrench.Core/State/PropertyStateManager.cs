using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.EventArg;
using velowrench.Core.Interfaces;
using velowrench.Utils.Enums;

namespace velowrench.Core.State;

/// <summary>
/// Manages the state of properties in a validation context
/// </summary>
public class PropertyStateManager<TInput> : IPropertyStateManager<TInput>
{
    private readonly Dictionary<string, EPropertyState> _propertyStates;
    private EValidationMode _validationMode;

    /// <inheritdoc />
    public EValidationMode ValidationMode
    {
        get => _validationMode;
        set => _validationMode = value;
    }

    /// <inheritdoc />
    public event EventHandler<EPropertyStateChangedEventArgs>? StateChanged;

    public PropertyStateManager(EValidationMode validationMode = EValidationMode.Progressive)
    {
        _validationMode = validationMode;
        _propertyStates = [];
    }

    /// <inheritdoc />
    public EPropertyState GetPropertyState(string propertyName)
    {
        return _propertyStates.TryGetValue(propertyName, out var state) ? state : EPropertyState.Untouched;
    }

    /// <inheritdoc />
    public void MarkPropertyTouched(string propertyName)
    {
        UpdatePropertyState(propertyName, EPropertyState.Touched);
    }

    /// <inheritdoc />
    public void MarkPropertyDirty(string propertyName)
    {
        UpdatePropertyState(propertyName, EPropertyState.Dirty);
    }

    /// <inheritdoc />
    public void MarkPropertyValidating(string propertyName)
    {
        UpdatePropertyState(propertyName, EPropertyState.Validating);
    }

    /// <inheritdoc />
    public void MarkPropertyValid(string propertyName)
    {
        UpdatePropertyState(propertyName, EPropertyState.Valid);
    }

    /// <inheritdoc />
    public void MarkPropertyInvalid(string propertyName)
    {
        UpdatePropertyState(propertyName, EPropertyState.Invalid);
    }

    /// <inheritdoc />
    public bool ShouldShowErrors(string propertyName)
    {
        var state = GetPropertyState(propertyName);

        return _validationMode switch
        {
            EValidationMode.Progressive => state is EPropertyState.Dirty or EPropertyState.Valid or EPropertyState.Invalid,
            EValidationMode.Immediate => state != EPropertyState.Untouched,
            EValidationMode.OnSubmit => false, // Errors shown only when explicitly requested
            _ => false
        };
    }

    /// <inheritdoc />
    public void Reset()
    {
        var changedProperties = _propertyStates.Keys.ToList();
        _propertyStates.Clear();

        foreach (var propertyName in changedProperties)
        {
            StateChanged?.Invoke(this, new EPropertyStateChangedEventArgs(propertyName, EPropertyState.Untouched, EPropertyState.Untouched));
        }
    }

    private void UpdatePropertyState(string propertyName, EPropertyState newState)
    {
        var previousState = GetPropertyState(propertyName);

        if (previousState != newState)
        {
            _propertyStates[propertyName] = newState;
            StateChanged?.Invoke(this, new EPropertyStateChangedEventArgs(propertyName, previousState, newState));
        }
    }
}
