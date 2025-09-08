using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using UnitsNet.Units;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;
using velowrench.Core.ViewModels.Help;

namespace velowrench.Core.ViewModels.Home;

/// <summary>
/// Represents the view model for managing user profile settings, including culture, theme, and unit preferences.
/// </summary>
public sealed partial class ProfileViewModel : BaseRoutableViewModel
{
    private readonly ILocalizer _localizer;
    private readonly IUnitStore _unitStore;

    /// <inheritdoc/>
    public override string Name { get; }

    [ObservableProperty]
    private List<CultureInfo> _supportedCultures;

    [ObservableProperty]
    private CultureInfo _selectedCulture;

    [ObservableProperty]
    private List<ThemeVariant> _availableThemes;

    [ObservableProperty]
    private ThemeVariant _selectedTheme;

    /// <summary>
    /// Gets or sets the user-preferred unit of length measurement.
    /// </summary>
    [ObservableProperty]
    private LengthUnit _selectedLengthUnit;

    /// <summary>
    /// Gets or sets the user-preferred unit of distance measurement.
    /// </summary>
    [ObservableProperty]
    private LengthUnit _selectedDistanceUnit;

    /// <summary>
    /// Gets or sets the user-preferred unit of speed measurement.
    /// </summary>
    [ObservableProperty]
    private SpeedUnit _selectedSpeedUnit;

    [ObservableProperty]
    private string _version;

    public IReadOnlyCollection<LengthUnit> LengthAvailableUnits => _unitStore.LengthAvailableUnits;
    public IReadOnlyCollection<LengthUnit> DistanceAvailableUnits => _unitStore.DistanceAvailableUnits;
    public IReadOnlyCollection<SpeedUnit> SpeedAvailableUnits => _unitStore.SpeedAvailableUnits;

    public ProfileViewModel(
        ILocalizer localizer,
        IUnitStore unitStore,
        INavigationService navigationService,
        IToolbar toolbar)
    : base(navigationService, toolbar)
    {
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        _unitStore = unitStore ?? throw new ArgumentNullException(nameof(unitStore));

        Name = _localizer.GetString("Settings");
    }

    /// <inheritdoc/>
    public override Task OnInitializedAsync()
    {
        this.SupportedCultures = [new("en-US"), new("fr-FR")];
        this.SelectedCulture = this.SupportedCultures.First();
        this.AvailableThemes = [ThemeVariant.Light, ThemeVariant.Dark, ThemeVariant.Default];
        this.SelectedTheme = Application.Current?.RequestedThemeVariant ??  ThemeVariant.Default;
        this.SelectedLengthUnit = _unitStore.LengthDefaultUnit;
        this.SelectedDistanceUnit = _unitStore.DistanceDefaultUnit;
        this.SelectedSpeedUnit = _unitStore.SpeedDefaultUnit;
        this.Version = Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString() ?? "N/A";
        return base.OnInitializedAsync();
    }

    [RelayCommand]
    private void ShowAboutPage()
    {
        using AboutViewModel vm = new(base.NavigationService, base.Toolbar, _localizer);
        base.NavigationService.NavigateToAsync(vm);
    }

    partial void OnSelectedThemeChanged(ThemeVariant value)
    {
        if (Application.Current != null && Application.Current.RequestedThemeVariant != value)
        {
            Application.Current.RequestedThemeVariant = value;
        }
    }

    partial void OnSelectedLengthUnitChanged(LengthUnit value)
    {
        _unitStore.LengthDefaultUnit = value;
    }

    partial void OnSelectedDistanceUnitChanged(LengthUnit value)
    {
        _unitStore.DistanceDefaultUnit = value;
    }

    partial void OnSelectedSpeedUnitChanged(SpeedUnit value)
    {
        _unitStore.SpeedDefaultUnit = value;
    }

    private bool _disposed;
    protected override void Dispose(bool disposing)
    {
        if (disposing && !_disposed)
        {
            _disposed = true;

            _supportedCultures?.Clear();
        }
        base.Dispose(disposing);
    }
}
