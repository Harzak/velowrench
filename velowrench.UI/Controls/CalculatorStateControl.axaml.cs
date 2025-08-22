using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
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
    /// Defines the ComputedContentTemplate property for custom content template when calculation is completed.
    /// </summary>
    public static readonly StyledProperty<IDataTemplate?> ComputedContentTemplateProperty =
        AvaloniaProperty.Register<CalculatorStateControl, IDataTemplate?>(nameof(ComputedContentTemplate));

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
    /// Gets or sets the data template to use for displaying content when the calculation is completed.
    /// </summary>
    public IDataTemplate? ComputedContentTemplate
    {
        get => GetValue(ComputedContentTemplateProperty);
        set => SetValue(ComputedContentTemplateProperty, value);
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