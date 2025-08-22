using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet.Units;
using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Calculators.Wheels.SpokeLength;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Units;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;
using velowrench.Repository.Interfaces;
using velowrench.Repository.Models;
using velowrench.Utils.Results;
using velowrench.Calculations.Units;
using System.ComponentModel;

namespace velowrench.Core.ViewModels.Tools;

public sealed partial class SpokeLengthCalculatorViewModel : BaseCalculatorViewModel<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult>
{
    private readonly IComponentStandardRepository _repository;

    /// <summary>
    /// Gets the display name of this view model.
    /// </summary>
    public override string Name { get; }

    /// <summary>
    /// Gets a value indicating whether this view model has an associated help page.
    /// </summary>
    public override bool CanShowHelpPage => true;

    [ObservableProperty]
    private ConvertibleDouble<LengthUnit>? _hubCenterToFlangeDistanceGearSide;

    [ObservableProperty]
    private ConvertibleDouble<LengthUnit>? _hubCenterToFlangeDistanceNonGearSide;

    [ObservableProperty]
    private ConvertibleDouble<LengthUnit>? _hubFlangeDiameterGearSide;

    [ObservableProperty]
    private ConvertibleDouble<LengthUnit>? _hubFlangeDiameterNonGearSide;

    [ObservableProperty]
    private LengthUnit _hubMeasurementsSelectedUnit;

    [ObservableProperty]
    private ConvertibleDouble<LengthUnit>? _rimInternalDiameter;

    [ObservableProperty]
    private LengthUnit _rimInternalDiameterSelectedUnit;

    [ObservableProperty]
    private IEnumerable<int> _availableSpokeCount;

    [ObservableProperty]
    private int _selectedSpokeCount;

    [ObservableProperty]
    private IEnumerable<SpokeLacingPatternModel> _availableSpokeLacingPatterns;

    [ObservableProperty]
    private SpokeLacingPatternModel _selectedSpokeLacingPattern;

    [ObservableProperty]
    private ConvertibleDouble<LengthUnit>? _recommendedSpokeLengthGearSide;

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
        };
    }

    protected override void OnCalculationSuccessful(OperationResult<SpokeLengthCalculatorResult> result)
    {
        this.RecommendedSpokeLengthGearSide = new ConvertibleDouble<LengthUnit>(result.Content.SpokeLengthGearSideMm, LengthUnit.Millimeter);
        this.RecommendedSpokeLengthNonGearSide = new ConvertibleDouble<LengthUnit>(result.Content.SpokeLengthNonGearSideMm, LengthUnit.Millimeter);
    }

    private void OnInputValueChanged(double value)
    {
        base.RefreshCalculationDebounced.Execute();
    }

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

    partial void OnRimInternalDiameterSelectedUnitChanging(LengthUnit value)
    {
        if (this.RimInternalDiameter != null)
        {
            this.RimInternalDiameter.Unit = value;
        }
    }

    partial void OnSelectedSpokeCountChanged(int value)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    partial void OnSelectedSpokeLacingPatternChanged(SpokeLacingPatternModel value)
    {
        base.RefreshCalculationDebounced.Execute();
    }
}

