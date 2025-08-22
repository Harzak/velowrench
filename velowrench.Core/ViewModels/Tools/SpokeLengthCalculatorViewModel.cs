using CommunityToolkit.Mvvm.ComponentModel;
using UnitsNet.Units;
using velowrench.Calculations.Calculators.Wheels.SpokeLength;
using velowrench.Calculations.Interfaces;
using velowrench.Calculations.Units;
using velowrench.Core.Interfaces;
using velowrench.Core.Units;
using velowrench.Core.ViewModels.Base;
using velowrench.Repository.Interfaces;
using velowrench.Repository.Models;
using velowrench.Utils.Results;

namespace velowrench.Core.ViewModels.Tools;

public sealed partial class SpokeLengthCalculatorViewModel : BaseCalculatorViewModel<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult>
{
    private readonly IComponentStandardRepository _repository;

    /// <summary>
    /// Gets the display name of this view model.
    /// </summary>
    public override string Name { get; }

    /// <summary>
    /// User-defined: Gets a value indicating whether this view model has an associated help page.
    /// </summary>
    /// <remarks>User-defined value</remarks>
    public override bool CanShowHelpPage => true;

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
    private IEnumerable<int> _availableSpokeCount;

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
    private IEnumerable<SpokeLacingPatternModel> _availableSpokeLacingPatterns;

    /// <summary>
    /// Gets the spoke lacing pattern (cross count). How many times each spoke crosses others (e.g., 3-cross, 2-cross, radial).
    /// </summary>
    /// <remarks>User-defined value</remarks>
    [ObservableProperty]
    private SpokeLacingPatternModel _selectedSpokeLacingPattern;

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
        ArgumentNullException.ThrowIfNull(localizer, nameof(localizer));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        _hubCenterToFlangeDistanceGearSide = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Millimeter, OnInputValueChanged);
        _hubCenterToFlangeDistanceNonGearSide = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Millimeter, OnInputValueChanged);
        _hubFlangeDiameterGearSide = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Millimeter, OnInputValueChanged);
        _hubFlangeDiameterNonGearSide = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Millimeter, OnInputValueChanged);
        _rimInternalDiameter = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Millimeter, OnInputValueChanged);

        _hubMeasurementsSelectedUnit = UnitsStore.WheelMeasurementsDefaultUnit;
        _rimInternalDiameterSelectedUnit = UnitsStore.WheelMeasurementsDefaultUnit;
        _availableSpokeCount = _repository.GetMostCommonWheelSpokeCount();
        _selectedSpokeCount = this.AvailableSpokeCount.First();
        _availableSpokeLacingPatterns = _repository.GetMostCommonSpokeLacingPattern();
        _selectedSpokeLacingPattern = this.AvailableSpokeLacingPatterns.First();

        this.Name = localizer.GetString("SpokeLengthCalculator");
    }

    /// <summary>
    /// Creates calculation input based on current view model state.
    /// Maps UI properties to the gear calculation input structure required by the calculation engine.
    /// </summary>
    protected override SpokeLengthCalculatorInput GetInput()
    {
        return new SpokeLengthCalculatorInput
        {
            HubCenterToFlangeDistanceGearSideMm = HubCenterToFlangeDistanceGearSide?.GetValueIn(LengthUnit.Millimeter) ?? 0,
            HubCenterToFlangeDistanceNonGearSideMm = HubCenterToFlangeDistanceNonGearSide?.GetValueIn(LengthUnit.Millimeter) ?? 0,
            HubFlangeDiameterGearSideMm = HubFlangeDiameterGearSide?.GetValueIn(LengthUnit.Millimeter) ?? 0,
            HubFlangeDiameterNonGearSideMm = HubFlangeDiameterNonGearSide?.GetValueIn(LengthUnit.Millimeter) ?? 0,
            RimInternalDiameterMm = RimInternalDiameter?.GetValueIn(LengthUnit.Millimeter) ?? 0,
            SpokeCount = this.SelectedSpokeCount,
            SpokeLacingPattern = this.SelectedSpokeLacingPattern.Crosses,
            Precision = 0
        };
    }

    /// <summary>
    /// Processes successful gear calculation results and updates the display data.
    /// </summary>
    protected override void OnCalculationSuccessful(OperationResult<SpokeLengthCalculatorResult> result)
    {
        this.RecommendedSpokeLengthGearSide = new ConvertibleDouble<LengthUnit>(result.Content.SpokeLengthGearSideMm, LengthUnit.Millimeter);
        this.RecommendedSpokeLengthNonGearSide = new ConvertibleDouble<LengthUnit>(result.Content.SpokeLengthNonGearSideMm, LengthUnit.Millimeter);
    }

    /// <summary>
    /// Handles changes to the input values (hub and rim measurement) and triggers a recalculation.
    /// </summary>
    private void OnInputValueChanged(double value)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    /// <summary>
    /// Updates the unit of measurement for all hub-related dimensions when the selected unit changes.
    /// </summary>
    partial void OnHubMeasurementsSelectedUnitChanged(LengthUnit value)
    {
        if (this.HubCenterToFlangeDistanceGearSide != null)
        {
            this.HubCenterToFlangeDistanceGearSide.Unit = value;
        }
        if (this.HubCenterToFlangeDistanceNonGearSide != null)
        {
            this.HubCenterToFlangeDistanceNonGearSide.Unit = value;
        }
        if (this.HubFlangeDiameterGearSide != null)
        {
            this.HubFlangeDiameterGearSide.Unit = value;
        }
        if (this.HubFlangeDiameterNonGearSide != null)
        {
            this.HubFlangeDiameterNonGearSide.Unit = value;
        }
    }

    /// <summary>
    /// Invoked when the selected unit for the rim's internal diameter is about to change.
    /// </summary>
    partial void OnRimInternalDiameterSelectedUnitChanging(LengthUnit value)
    {
        if (this.RimInternalDiameter != null)
        {
            this.RimInternalDiameter.Unit = value;
        }
    }

    /// <summary>
    /// Invoked when the selected spoke count changes.
    /// </summary>
    partial void OnSelectedSpokeCountChanged(int value)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    /// <summary>
    /// Handles changes to the selected spoke lacing pattern.
    /// </summary>
    partial void OnSelectedSpokeLacingPatternChanged(SpokeLacingPatternModel value)
    {
        base.RefreshCalculationDebounced.Execute();
    }
}

