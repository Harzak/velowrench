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
using velowrench.Calculations.Units;
using velowrench.Core.Interfaces;
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
    /// Gets a value indicating whether this view model has an associated help page.
    /// </summary>
    public override bool CanShowHelpPage => true;

    [ObservableProperty]
    private ConvertibleDouble<LengthUnit> _hubCenterToFlangeDistanceGearSide;

    [ObservableProperty]
    private ConvertibleDouble<LengthUnit> _hubCenterToFlangeDistanceNonGearSide;

    [ObservableProperty]
    private ConvertibleDouble<LengthUnit> _hubFlangeDiameterGearSide;

    [ObservableProperty]
    private ConvertibleDouble<LengthUnit> _hubFlangeDiameterNonGearSide;

    [ObservableProperty]
    private LengthUnit _hubMeasurementsSelectedUnit;

    [ObservableProperty]
    private ConvertibleDouble<LengthUnit> _rimInternalDiameter;

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
    private ConvertibleDouble<LengthUnit> _recommendedSpokeLengthGearSide;

    [ObservableProperty]
    private ConvertibleDouble<LengthUnit> _recommendedSpokeLengthNonGearSide;

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

        this.HubCenterToFlangeDistanceGearSide = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Millimeter);
        this.HubCenterToFlangeDistanceNonGearSide = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Millimeter);
        this.HubFlangeDiameterGearSide = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Millimeter);
        this.HubFlangeDiameterNonGearSide = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Millimeter);
        this.RimInternalDiameter = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Millimeter);
        this.HubMeasurementsSelectedUnit = UnitsStore.WheelMeasurementsDefaultUnit;
        this.RimInternalDiameterSelectedUnit = UnitsStore.WheelMeasurementsDefaultUnit;
        this.AvailableSpokeCount = _repository.GetMostCommonWheelSpokeCount();
        this.SelectedSpokeCount = this.AvailableSpokeCount.First();
        this.AvailableSpokeLacingPatterns = _repository.GetMostCommonSpokeLacingPattern();
        this.SelectedSpokeLacingPattern = this.AvailableSpokeLacingPatterns.First();
        this.Name = localizer.GetString("SpokeLengthCalculator");
    }

    protected override SpokeLengthCalculatorInput GetInput()
    {
        return new SpokeLengthCalculatorInput
        {
            HubCenterToFlangeDistanceGearSide = this.HubCenterToFlangeDistanceGearSide,
            HubCenterToFlangeDistanceNonGearSide = this.HubCenterToFlangeDistanceNonGearSide,
            HubFlangeDiameterGearSide = this.HubFlangeDiameterGearSide,
            HubFlangeDiameterNonGearSide = this.HubFlangeDiameterNonGearSide,
            RimInternalDiameter = this.RimInternalDiameter,
            SpokeCount = this.SelectedSpokeCount,
            SpokeLacingPattern = this.SelectedSpokeLacingPattern.Crosses,
        };
    }

    protected override void OnCalculationSuccessful(OperationResult<SpokeLengthCalculatorResult> result)
    {
        this.RecommendedSpokeLengthGearSide = result.Content.SpokeLengthGearSide;
        this.RecommendedSpokeLengthNonGearSide = result.Content.SpokeLengthNonGearSide;
    }

    partial void OnHubMeasurementsSelectedUnitChanging(LengthUnit value)
    {
        this.HubCenterToFlangeDistanceGearSide.Unit = value;
        this.HubCenterToFlangeDistanceNonGearSide.Unit = value;
        this.HubFlangeDiameterGearSide.Unit = value;
        this.HubFlangeDiameterNonGearSide.Unit = value;
    }

    partial void OnHubCenterToFlangeDistanceGearSideChanged(ConvertibleDouble<LengthUnit> value)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    partial void OnHubCenterToFlangeDistanceNonGearSideChanged(ConvertibleDouble<LengthUnit> value)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    partial void OnHubFlangeDiameterGearSideChanged(ConvertibleDouble<LengthUnit> value)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    partial void OnHubFlangeDiameterNonGearSideChanged(ConvertibleDouble<LengthUnit> value)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    partial void OnRimInternalDiameterChanged(ConvertibleDouble<LengthUnit> value)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    partial void OnRimInternalDiameterSelectedUnitChanging(LengthUnit value)
    {
        this.RimInternalDiameter.Unit = value;
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

