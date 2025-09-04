using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UnitsNet.Units;
using velowrench.Calculations.Calculators.Wheels.SpokeLength;
using velowrench.Calculations.Interfaces;
using velowrench.Calculations.Units;
using velowrench.Core.Interfaces;
using velowrench.Core.Units;
using velowrench.Core.ViewModels.Base;
using velowrench.Core.ViewModels.Help;
using velowrench.Repository.Interfaces;
using velowrench.Repository.Models;
using velowrench.Utils.Interfaces;
using velowrench.Utils.Results;

namespace velowrench.Core.ViewModels.Tools;

public sealed partial class SpokeLengthCalculatorViewModel : BaseCalculatorViewModel<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult>
{
    private readonly ILocalizer _localizer;
    private readonly IComponentStandardRepository _repository;

    /// <inheritdoc/>
    protected override SpokeLengthCalculatorInput CalculatorInput { get; set; }

    /// <inheritdoc/>
    public override string Name { get; }

    /// <inheritdoc/>
    public override bool CanShowHelpPage => true;

    /// <inheritdoc/>
    public override bool CanShowContextMenu => true;

    /// <summary>
    /// User-defined: Gets the distance from the hub center to the flange on the gear (right) side.
    /// </summary>
    /// <remarks>User-defined value</remarks>
    [ObservableProperty]
    private ConvertibleDouble<LengthUnit>? _hubCenterToFlangeDistanceGearSide;

    /// <summary>
    /// User-defined: Gets the distance from the hub center to the flange on the non-gear (left) side.
    /// </summary>
    /// <remarks>User-defined value</remarks>
    [ObservableProperty]
    private ConvertibleDouble<LengthUnit>? _hubCenterToFlangeDistanceNonGearSide;

    /// <summary>
    /// User-defined: Gets the diameter of the circle through the centers of the spoke holes on the gear side (right) flange.
    /// </summary>
    /// <remarks>User-defined value</remarks>
    [ObservableProperty]
    private ConvertibleDouble<LengthUnit>? _hubFlangeDiameterGearSide;

    /// <summary>
    /// User-defined: Gets the diameter of the circle through the centers of the spoke holes on the non-gear side (left) flange.
    /// </summary>
    /// <remarks>User-defined value</remarks>
    [ObservableProperty]
    private ConvertibleDouble<LengthUnit>? _hubFlangeDiameterNonGearSide;

    [ObservableProperty]
    private LengthUnit _hubMeasurementsSelectedUnit;

    /// <summary>
    /// User-defined: Gets the internal diameter / effective Rim Diameter (ERD) of the rim. The diameter measured at the nipple seats inside the rim
    /// </summary>
    /// <remarks>User-defined value</remarks>
    [ObservableProperty]
    private ConvertibleDouble<LengthUnit>? _rimInternalDiameter;

    /// <summary>
    /// Gets or sets the selected unit of measurement for the rim's internal diameter.
    /// </summary>
    /// <remarks>User-defined unit</remarks>
    [ObservableProperty]
    private LengthUnit _rimInternalDiameterSelectedUnit;

    /// <summary>
    /// Gets or sets the collection of available spoke counts.
    /// </summary>
    [ObservableProperty]
    private List<int> _availableSpokeCount;

    /// <summary>
    /// Gets the number of spokes in the wheel (hole count).
    /// </summary>
    /// <remarks>User-defined value</remarks>
    [ObservableProperty]
    private int _selectedSpokeCount;

    /// <summary>
    /// Gets the collection of available spoke lacing patterns.
    /// </summary>
    [ObservableProperty]
    private List<SpokeLacingPatternModel> _availableSpokeLacingPatterns;

    /// <summary>
    /// Gets the spoke lacing pattern (cross count). How many times each spoke crosses others (e.g., 3-cross, 2-cross, radial).
    /// </summary>
    /// <remarks>User-defined value</remarks>
    [ObservableProperty]
    private SpokeLacingPatternModel? _selectedSpokeLacingPattern;

    /// <summary>
    ///  Gets or sets the recommended spoke length for the gear side, convertible in a specified length unit.
    /// </summary>
    /// <remarks>Calculation result value</remarks>
    [ObservableProperty]
    private ConvertibleDouble<LengthUnit>? _recommendedSpokeLengthGearSide;

    /// <summary>
    ///  Gets or sets the recommended spoke length for the non-gear side, convertible in a specified length unit.
    /// </summary>
    /// <remarks>Calculation result value</remarks>
    [ObservableProperty]
    private ConvertibleDouble<LengthUnit>? _recommendedSpokeLengthNonGearSide;

    public SpokeLengthCalculatorViewModel(
        ICalculatorFactory<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult> calculatorFactory,
        INavigationService navigationService,
        IDebounceActionFactory actionFactory,
                IComponentStandardRepository repository,
        ILocalizer localizer)
    : base(calculatorFactory, navigationService, actionFactory)
    {
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        this.Name = localizer.GetString("SpokeLengthCalculator");
        this.CalculatorInput = new SpokeLengthCalculatorInput(precision: 0);
    }

    public override Task OnInitializedAsync()
    {
        this.FillDefaultValues();
        return base.OnInitializedAsync();
    }

    /// Updates default values with arbitrary values until user configuration management is implemented.
    private void FillDefaultValues()
    {
        base.WithProgrammaticChange(() =>
        {
            this.HubCenterToFlangeDistanceGearSide = new ConvertibleDouble<LengthUnit>(LengthUnit.Millimeter, OnHubCenterToFlangeDistanceGearSideChanged);
            this.HubCenterToFlangeDistanceNonGearSide = new ConvertibleDouble<LengthUnit>(LengthUnit.Millimeter, OnHubCenterToFlangeDistanceNonGearSideChanged);
            this.HubFlangeDiameterGearSide = new ConvertibleDouble<LengthUnit>(LengthUnit.Millimeter, OnHubFlangeDiameterGearSideChanged);
            this.HubFlangeDiameterNonGearSide = new ConvertibleDouble<LengthUnit>(LengthUnit.Millimeter, OnHubFlangeDiameterNonGearSideChanged);
            this.RimInternalDiameter = new ConvertibleDouble<LengthUnit>(LengthUnit.Millimeter, OnRimInternalDiameterChanged);

            this.HubMeasurementsSelectedUnit = UnitsStore.WheelMeasurementsDefaultUnit;
            this.RimInternalDiameterSelectedUnit = UnitsStore.WheelMeasurementsDefaultUnit;
            this.AvailableSpokeCount = _repository.GetMostCommonWheelSpokeCount().ToList();
            this.SelectedSpokeCount = this.AvailableSpokeCount.First(x => x == 32);
            this.AvailableSpokeLacingPatterns = _repository.GetMostCommonSpokeLacingPattern().ToList();
            this.SelectedSpokeLacingPattern = this.AvailableSpokeLacingPatterns.First(x => x.Crosses == 3);
        });
    }

    partial void OnHubCenterToFlangeDistanceGearSideChanged(ConvertibleDouble<LengthUnit>? value)
    {
        this.CalculatorInput.HubCenterToFlangeDistanceGearSideMm = value?.GetValueIn(LengthUnit.Millimeter) ?? 0;
        base.OnCalculationInputChanged(nameof(this.CalculatorInput.HubCenterToFlangeDistanceGearSideMm));
    }

    partial void OnHubCenterToFlangeDistanceNonGearSideChanged(ConvertibleDouble<LengthUnit>? value)
    {
        this.CalculatorInput.HubCenterToFlangeDistanceNonGearSideMm = value?.GetValueIn(LengthUnit.Millimeter) ?? 0;
        base.OnCalculationInputChanged(nameof(this.CalculatorInput.HubCenterToFlangeDistanceNonGearSideMm));
    }

    partial void OnHubFlangeDiameterGearSideChanged(ConvertibleDouble<LengthUnit>? value)
    {
        this.CalculatorInput.HubFlangeDiameterGearSideMm = value?.GetValueIn(LengthUnit.Millimeter) ?? 0;
        base.OnCalculationInputChanged(nameof(this.CalculatorInput.HubFlangeDiameterGearSideMm));
    }

    partial void OnHubFlangeDiameterNonGearSideChanged(ConvertibleDouble<LengthUnit>? value)
    {
        this.CalculatorInput.HubFlangeDiameterNonGearSideMm = value?.GetValueIn(LengthUnit.Millimeter) ?? 0;
        base.OnCalculationInputChanged(nameof(this.CalculatorInput.HubFlangeDiameterNonGearSideMm));
    }

    partial void OnRimInternalDiameterChanged(ConvertibleDouble<LengthUnit>? value)
    {
        this.CalculatorInput.RimInternalDiameterMm = value?.GetValueIn(LengthUnit.Millimeter) ?? 0;
        base.OnCalculationInputChanged(nameof(this.CalculatorInput.RimInternalDiameterMm));
    }

    partial void OnSelectedSpokeCountChanged(int value)
    {
        this.CalculatorInput.SpokeCount = value;
        base.OnCalculationInputChanged(nameof(this.CalculatorInput.SpokeCount));
    }

    partial void OnSelectedSpokeLacingPatternChanged(SpokeLacingPatternModel? value)
    {
        this.CalculatorInput.SpokeLacingPattern = value?.Crosses ?? 0;
        base.OnCalculationInputChanged(nameof(this.CalculatorInput.SpokeLacingPattern));
    }

    protected override void OnCalculationSuccessful(OperationResult<SpokeLengthCalculatorResult> result)
    {
        this.RecommendedSpokeLengthGearSide = new ConvertibleDouble<LengthUnit>(result.Content.SpokeLengthGearSideMm, LengthUnit.Millimeter);
        this.RecommendedSpokeLengthNonGearSide = new ConvertibleDouble<LengthUnit>(result.Content.SpokeLengthNonGearSideMm, LengthUnit.Millimeter);
    }

    partial void OnHubMeasurementsSelectedUnitChanged(LengthUnit value)
    {
        if (this.HubCenterToFlangeDistanceGearSide != null)
        {
            this.HubCenterToFlangeDistanceGearSide.Unit = value;
            OnPropertyChanged(nameof(this.HubCenterToFlangeDistanceGearSide));
        }
        if (this.HubCenterToFlangeDistanceNonGearSide != null)
        {
            this.HubCenterToFlangeDistanceNonGearSide.Unit = value;
            OnPropertyChanged(nameof(this.HubCenterToFlangeDistanceNonGearSide));
        }
        if (this.HubFlangeDiameterGearSide != null)
        {
            this.HubFlangeDiameterGearSide.Unit = value;
            OnPropertyChanged(nameof(this.HubFlangeDiameterGearSide));
        }
        if (this.HubFlangeDiameterNonGearSide != null)
        {
            this.HubFlangeDiameterNonGearSide.Unit = value;
            OnPropertyChanged(nameof(this.HubFlangeDiameterNonGearSide));
        }
    }

    partial void OnRimInternalDiameterSelectedUnitChanged(LengthUnit value)
    {
        if (this.RimInternalDiameter != null)
        {
            this.RimInternalDiameter.Unit = value;
            OnPropertyChanged(nameof(this.RimInternalDiameter));
        }
    }

    [RelayCommand]
    public override void ShowHelpPage()
    {
        base.NavigationService.NavigateToHelpAsync(Enums.EVeloWrenchTools.SpokeLengthCalculator);
    }

    [RelayCommand]
    private void ShowHubMeasurementsHelpPage()
    {
        using SpokeLengthCalculatorHubMeasurementHelpViewModel vm = new(NavigationService, _localizer);
        base.NavigationService.NavigateToAsync(vm);
    }

    [RelayCommand]
    private void ShowRimMeasurementsHelpPage()
    {
        using SpokeLengthCalculatorRimMeasurementHelpViewModel vm = new(NavigationService, _localizer);
        base.NavigationService.NavigateToAsync(vm);
    }

    [RelayCommand]
    private void ShowBuildConfigurationHelpPage()
    {
        using SpokeLengthCalculatorBuildConfigurationHelpViewModel vm = new(NavigationService, _localizer);
        base.NavigationService.NavigateToAsync(vm);
    }

    public override void ResetToDefault()
    {
        this.CalculatorInput = new SpokeLengthCalculatorInput(precision: 0);
        this.FillDefaultValues();
        base.ResetToDefault();
    }

    private bool _disposed;
    protected override void Dispose(bool disposing)
    {
        if (disposing && !_disposed)
        {
            _availableSpokeCount?.Clear();
            _availableSpokeLacingPatterns?.Clear();
            _hubCenterToFlangeDistanceGearSide = null;
            _hubCenterToFlangeDistanceNonGearSide = null;
            _hubFlangeDiameterGearSide = null;
            _hubFlangeDiameterNonGearSide = null;
            _rimInternalDiameter = null;
            _recommendedSpokeLengthGearSide = null;
            _recommendedSpokeLengthNonGearSide = null;

            _disposed = true;
        }

        base.Dispose(disposing);
    }
}

