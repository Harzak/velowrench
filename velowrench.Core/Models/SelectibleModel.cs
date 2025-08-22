namespace velowrench.Core.Models;

/// <summary>
/// Provides a wrapper around any object to add selection state functionality.
/// This generic model enables UI components to track whether an item is selected
/// while preserving the original object's data and functionality.
/// </summary>
public class SelectibleModel<T> where T : class
{
    /// <summary>
    /// Gets or sets a value indicating whether this item is currently selected.
    /// This property is typically bound to UI controls like checkboxes or list selections.
    /// </summary>
    public bool IsSelected { get; set; }

    /// <summary>
    /// Gets or sets the wrapped object that contains the actual data.
    /// </summary>
    public T Value { get; set; }

    public SelectibleModel(T value)
    {
        this.Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public SelectibleModel(T value, bool isSelected) : this(value)
    {
        this.IsSelected = isSelected;
    }
}
