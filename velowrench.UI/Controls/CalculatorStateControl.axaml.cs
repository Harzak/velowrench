using Avalonia;
using Avalonia.Controls;
using velowrench.Utils.Enums;

namespace velowrench.UI.Controls;

public partial class CalculatorStateControl : UserControl
{
    /// <summary>
    /// Defines the State property.
    /// </summary>
    public static readonly StyledProperty<ECalculatorState> StateProperty =
        AvaloniaProperty.Register<CalculatorStateControl, ECalculatorState>(nameof(State));

    /// <summary>
    /// Defines the ComputedContent property for custom content when calculation is completed.
    /// </summary>
    public static readonly StyledProperty<object?> ComputedContentProperty =
        AvaloniaProperty.Register<CalculatorStateControl, object?>(nameof(ComputedContent));

    /// <summary>
    /// Defines the ErrorMessage property for displaying validation errors.
    /// </summary>
    public static readonly StyledProperty<string?> ErrorMessageProperty =
        AvaloniaProperty.Register<CalculatorStateControl, string?>(nameof(ErrorMessage));

    /// <summary>
    /// Gets or sets the current calculator state.
    /// </summary>
    public ECalculatorState State
    {
        get => GetValue(StateProperty);
        set => SetValue(StateProperty, value);
    }

    /// <summary>
    /// Gets or sets the custom content to display when the calculation is completed.
    /// </summary>
    public object? ComputedContent
    {
        get => GetValue(ComputedContentProperty);
        set => SetValue(ComputedContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the error message to display when validation fails.
    /// </summary>
    public string? ErrorMessage
    {
        get => GetValue(ErrorMessageProperty);
        set => SetValue(ErrorMessageProperty, value);
    }

    public CalculatorStateControl()
    {
        InitializeComponent();
    }
}