using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using velowrench.Calculations.Calculators.Transmission.Gear;
using velowrench.Calculations.Enums;
using velowrench.Calculations.Interfaces;
using velowrench.Calculations.Units;
using velowrench.Core.Interfaces;
using velowrench.Core.Models;
using velowrench.Core.ViewModels.Base;
using velowrench.Repository.Extensions;
using velowrench.Repository.Interfaces;
using velowrench.Repository.Models;
using velowrench.Utils.Results;

namespace velowrench.Core.ViewModels.Tools;

/// <summary>
/// View model for the gear calculator tool that performs bicycle gear ratio calculations.
/// Provides gear analysis including gear inches, development, gain ratio, and speed calculations
/// for various chainring and sprocket combinations.
/// </summary>
public sealed partial class GearCalculatorViewModel : BaseCalculatorViewModel<GearCalculatorInput, GearCalculatorResult>
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

    /// <summary>
    /// Gets all available gear calculation types for user selection.
    /// </summary>
    public IEnumerable<EGearCalculatorType> CalculationTypes => Enum.GetValues<EGearCalculatorType>();

    /// <summary>
    /// Formatted string representation of currently selected sprockets.
    /// Used for UI display and user feedback about current sprocket selection.
    /// </summary>
    [ObservableProperty]
    private string? _selectedSprocketsStr;

    /// <summary>
    /// Gets or sets the type of gear calculation to perform.
    /// Determines which calculation algorithm is used and which optional parameters are required.
    /// </summary>
    [ObservableProperty]
    private EGearCalculatorType _selectedCalculatorType;

    /// <summary>
    /// Gets or sets the collection of available wheel specifications.
    /// Contains standard bicycle wheel sizes with their corresponding measurements.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<WheelSpecificationModel> _sourceWheels;

    /// <summary>
    /// Gets or sets the currently selected wheel specification.
    /// </summary>
    [ObservableProperty]
    private WheelSpecificationModel _selectedWheel;

    /// <summary>
    /// Gets or sets the collection of available crankset specifications.
    /// Contains standard crank arm lengths used for gain ratio calculations.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<CranksetSpecificationModel> _sourceCranks;

    /// <summary>
    /// Gets or sets the currently selected crankset specification.
    /// Required when calculation type is <see cref="EGearCalculatorType.GainRatio"/>.
    /// </summary>
    [ObservableProperty]
    private CranksetSpecificationModel _selectedCrank;

    /// <summary>
    /// Gets or sets the collection of available cadence values.
    /// Contains common pedaling rates in revolutions per minute for speed calculations.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<CadenceModel> _sourceCadence;

    /// <summary>
    /// Gets or sets the currently selected cadence value.
    /// Required when calculation type is <see cref="EGearCalculatorType.Speed"/>.
    /// </summary>
    [ObservableProperty]
    private CadenceModel _selectedCadence;

    /// <summary>
    /// Gets or sets the collection of available sprocket specifications with selection state.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<SelectibleModel<SprocketSpecificationModel>> _sourceSprockets;

    /// <summary>
    /// Gets or sets the number of teeth on the primary (largest or only) chainring.
    /// </summary>
    [ObservableProperty]
    [Range(10, 120)]
    private int? _chainring1TeethCount;

    /// <summary>
    /// Gets or sets the number of teeth on the medium chainring (optional).
    /// Used in double and triple chainring configurations for intermediate gear ratios.
    /// </summary>
    [ObservableProperty]
    [Range(10, 120)]
    private int? _chainring2TeethCount;

    /// <summary>
    /// Gets or sets the number of teeth on the smallest chainring (optional).
    /// Used in double and triple chainring configurations for the lowest gear ratios.
    /// </summary>
    [ObservableProperty]
    [Range(10, 120)]
    private int? _chainring3TeethCount;

    /// <summary>
    /// Gets or sets the collection of units that can be applied to the calculation results.
    /// </summary>
    [ObservableProperty]
    ObservableCollection<Enum> _availableResultUnits;

    /// <summary>
    /// Gets or sets the unit in which results are displayed.
    /// </summary>
    [ObservableProperty]
    private Enum? _selectedResultUnit;

    /// <summary>
    /// Gets or sets the collection of calculated gear data for display.
    /// </summary>
    [ObservableProperty]
    ObservableCollection<GearCalculResultRowModel> _gearCalculResultRows;

    public GearCalculatorViewModel(
        ICalculatorFactory<GearCalculatorInput, GearCalculatorResult> calculatorFactory,
        INavigationService navigationService,
        IDebounceActionFactory actionFactory,
        IComponentStandardRepository repository,
        ILocalizer localizer)
    : base(calculatorFactory, navigationService, actionFactory)
    {
        ArgumentNullException.ThrowIfNull(localizer, nameof(localizer));
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));

        _repository = repository;

        _selectedCalculatorType = EGearCalculatorType.GearInches;
        _sourceWheels = new(_repository.GetMostCommonWheelSpecifications());
        _selectedWheel = this.SourceWheels.GetMostUsedWheel();
        _sourceCranks = new(_repository.GetAllCranksetSpecifications());
        _selectedCrank = this.SourceCranks.GetMostUsedCrankset();
        _sourceCadence = new(_repository.GetAllCandences());
        _selectedCadence = this.SourceCadence.GetMostUsedCadence();
        _sourceSprockets = new(_repository.GetMostCommonSprocketSpecifications().Select(x => new SelectibleModel<SprocketSpecificationModel>(x)));
        _availableResultUnits = [];
        _gearCalculResultRows = [];
        _chainring1TeethCount = 10;

        this.Name = localizer.GetString("GearCalculator");
    }

    /// <summary>
    /// Creates calculation input based on current view model state.
    /// Maps UI properties to the gear calculation input structure required by the calculation engine.
    /// </summary>
    protected override GearCalculatorInput GetInput()
    {
        return new GearCalculatorInput()
        {
            CalculatorType = this.SelectedCalculatorType,
            TeethNumberLargeOrUniqueChainring = this.Chainring1TeethCount ?? 0,
            TeethNumberMediumChainring = this.Chainring2TeethCount,
            TeethNumberSmallChainring = this.Chainring3TeethCount,
            NumberOfTeethBySprocket = [.. this.SourceSprockets.Where(x => x.IsSelected).OrderBy(x => x.Value.TeethCount).Select(x => x.Value.TeethCount)],
            TyreOuterDiameterIn = this.SelectedWheel.TyreOuterDiameterInInch,
            CrankLengthMm = this.SelectedCrank.Length,
            RevolutionPerMinute = this.SelectedCadence.Rpm,
            Precision = 2
        };
    }

    /// <summary>
    /// Processes successful gear calculation results and updates the display data.
    /// </summary>
    protected override void OnCalculationSuccessful(OperationResult<GearCalculatorResult> result)
    {
        this.GearCalculResultRows.Clear();
        for (int i = 0; i < result.Content.ValuesLargeOrUniqueChainring.Count; i++)
        {
            this.GearCalculResultRows.Add(new GearCalculResultRowModel(result.Content.ValuesLargeOrUniqueChainring[i], result.Content.Unit)
            {
                ValueForChainring2 = result.Content.ValuesMediumChainring?.Count > i ? result.Content.ValuesMediumChainring[i] : null,
                ValueForChainring3 = result.Content.ValuesSmallChainring?.Count > i ? result.Content.ValuesSmallChainring[i] : null,
                SprocketCount = result.Content.UsedInputs.NumberOfTeethBySprocket[i],
                Intensity = GearCalculatorResultInterpreter.DetermineIntensity(result.Content.ValuesLargeOrUniqueChainring[i], result.Content.UsedInputs.CalculatorType),
                Precision = result.Content.UsedInputs.Precision,
            });
        }

        this.AvailableResultUnits.Clear();
        foreach (var item in UnitsStore.GetAvailableUnitForGearCalculation(result.Content.UsedInputs.CalculatorType))
        {
            this.AvailableResultUnits.Add(item);
        }
        this.SelectedResultUnit = result.Content.Unit;
    }

    /// <summary>
    /// Handles sprocket selection changes in the user interface.
    /// </summary>
    [RelayCommand]
    private void SprocketSelected(SelectibleModel<SprocketSpecificationModel> selectibleSprocket)
    {
        SelectedSprocketsStr =  string.Join(", ", SourceSprockets.Where(x => x.IsSelected).OrderBy(x => x.Value.TeethCount).Select(x => x.Value.Label));
        base.RefreshCalculationDebounced.Execute();
    }

    /// <summary>
    /// Handles changes to the calculation type selection.
    /// Triggers input validation and potential recalculation when the calculation method changes.
    /// </summary>
    partial void OnSelectedCalculatorTypeChanged(EGearCalculatorType value)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    /// <summary>
    /// Handles changes to the primary chainring teeth count.
    /// </summary>
    partial void OnChainring1TeethCountChanged(int? value)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    /// <summary>
    /// Handles changes to the medium chainring teeth count.
    /// </summary>
    partial void OnChainring2TeethCountChanged(int? value)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    /// <summary>
    /// Handles changes to the smallest chainring teeth count.
    /// </summary>
    partial void OnChainring3TeethCountChanged(int? value)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    /// <summary>
    /// Handles changes to the selected cadence value.
    /// </summary>
    partial void OnSelectedCadenceChanged(CadenceModel value)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    /// <summary>
    /// Handles changes to the selected wheel specification.
    /// </summary>
    partial void OnSelectedWheelChanged(WheelSpecificationModel value)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    /// <summary>
    /// Handles changes to the selected crankset specification.
    /// </summary>
    partial void OnSelectedCrankChanged(CranksetSpecificationModel value)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    /// <summary>
    /// Handles changes to the selected result unit and updates the unit of all gear calculation result rows accordingly.
    /// </summary>
    partial void OnSelectedResultUnitChanged(Enum? value)
    {
        if (value != null && this.GearCalculResultRows.Count > 0 && this.GearCalculResultRows.First().ValueUnit != value)
        {
            foreach (GearCalculResultRowModel row in this.GearCalculResultRows)
            {
                row.ValueUnit = value;
            }
        }
    }

    /// <summary>
    /// Shows the help page for the gear calculator.
    /// </summary>
    [RelayCommand]
    public override void ShowHelpPage()
    {
        base.NavigationService.NavigateToHelp(Enums.EVeloWrenchTools.GearCalculator);
    }
}