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

        _hubCenterToFlangeDistanceGearSide = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Millimeter);
        _hubCenterToFlangeDistanceNonGearSide = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Millimeter);
        _hubFlangeDiameterGearSide = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Millimeter);
        _hubFlangeDiameterNonGearSide = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Millimeter);
        _rimInternalDiameter = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Millimeter);

        _hubCenterToFlangeDistanceGearSide.ValueChanged += this.OnHubCenterToFlangeDistanceGearSideChanged;
        _hubCenterToFlangeDistanceNonGearSide.ValueChanged += this.OnHubCenterToFlangeDistanceNonGearSideChanged;
        _hubFlangeDiameterGearSide.ValueChanged += this.OnHubFlangeDiameterGearSideChanged;
        _hubFlangeDiameterNonGearSide.ValueChanged += this.OnHubFlangeDiameterNonGearSideChanged;
        _rimInternalDiameter.ValueChanged += this.OnRimInternalDiameterChanged;

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
            HubCenterToFlangeDistanceGearSide = this.HubCenterToFlangeDistanceGearSide != null ? this.HubCenterToFlangeDistanceGearSide.Clone() : ConvertibleDouble<LengthUnit>.Default(),
            HubCenterToFlangeDistanceNonGearSide = this.HubCenterToFlangeDistanceNonGearSide != null ? this.HubCenterToFlangeDistanceNonGearSide.Clone() : ConvertibleDouble<LengthUnit>.Default(),
            HubFlangeDiameterGearSide = this.HubFlangeDiameterGearSide != null ? this.HubFlangeDiameterGearSide.Clone() : ConvertibleDouble<LengthUnit>.Default(),
            HubFlangeDiameterNonGearSide = this.HubFlangeDiameterNonGearSide != null ? this.HubFlangeDiameterNonGearSide.Clone() : ConvertibleDouble<LengthUnit>.Default(),
            RimInternalDiameter = this.RimInternalDiameter != null ? this.RimInternalDiameter.Clone() : ConvertibleDouble<LengthUnit>.Default(),
            SpokeCount = this.SelectedSpokeCount,
            SpokeLacingPattern = this.SelectedSpokeLacingPattern.Crosses,
        };
    }

    protected override void OnCalculationSuccessful(OperationResult<SpokeLengthCalculatorResult> result)
    {
        this.RecommendedSpokeLengthGearSide = result.Content.SpokeLengthGearSide;
        this.RecommendedSpokeLengthNonGearSide = result.Content.SpokeLengthNonGearSide;
    }

    partial void OnHubMeasurementsSelectedUnitChanged(LengthUnit value)
    {
        if (this.HubCenterToFlangeDistanceGearSide != null)
        {
            this.HubCenterToFlangeDistanceGearSide.Unit = value;
            base.OnPropertyChanged(nameof(HubCenterToFlangeDistanceGearSide));
        }
        if (this.HubCenterToFlangeDistanceNonGearSide != null)
        {
            this.HubCenterToFlangeDistanceNonGearSide.Unit = value;
            base.OnPropertyChanged(nameof(HubCenterToFlangeDistanceNonGearSide));

        }
        if (this.HubFlangeDiameterGearSide != null)
        {
            this.HubFlangeDiameterGearSide.Unit = value;
            base.OnPropertyChanged(nameof(HubFlangeDiameterGearSide));

        }
        if (this.HubFlangeDiameterNonGearSide != null)
        {
            this.HubFlangeDiameterNonGearSide.Unit = value;
            base.OnPropertyChanged(nameof(HubFlangeDiameterNonGearSide.Value));

        }
    }

    private void OnRimInternalDiameterChanged(object? sender, EventArgs e)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    private void OnHubFlangeDiameterNonGearSideChanged(object? sender, EventArgs e)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    private void OnHubFlangeDiameterGearSideChanged(object? sender, EventArgs e)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    private void OnHubCenterToFlangeDistanceNonGearSideChanged(object? sender, EventArgs e)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    private void OnHubCenterToFlangeDistanceGearSideChanged(object? sender, EventArgs e)
    {
        base.RefreshCalculationDebounced.Execute();
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

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (this.HubCenterToFlangeDistanceGearSide != null)
            {
                this.HubCenterToFlangeDistanceGearSide.ValueChanged -= this.OnHubCenterToFlangeDistanceGearSideChanged;
            }
            if (this.HubCenterToFlangeDistanceNonGearSide != null)
            {
                this.HubCenterToFlangeDistanceNonGearSide.ValueChanged -= this.OnHubCenterToFlangeDistanceNonGearSideChanged;
            }
            if (this.HubFlangeDiameterGearSide != null)
            {
                this.HubFlangeDiameterGearSide.ValueChanged -= this.OnHubFlangeDiameterGearSideChanged;
            }
            if (this.HubFlangeDiameterNonGearSide != null)
            {
                this.HubFlangeDiameterNonGearSide.ValueChanged -= this.OnHubFlangeDiameterNonGearSideChanged;
            }
            if (this.RimInternalDiameter != null)
            {
                this.RimInternalDiameter.ValueChanged -= this.OnRimInternalDiameterChanged;
            }

        }
        base.Dispose(disposing);
    }
}

