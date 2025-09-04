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
using velowrench.Utils.Interfaces;
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

    /// <inheritdoc/>
    protected override GearCalculatorInput CalculatorInput { get; }

    /// <inheritdoc/>
    public override string Name { get; }

    /// <inheritdoc/>
    public override bool CanShowHelpPage => true;

    /// <inheritdoc/>
    public override bool CanShowContextMenu => true;

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
    private WheelSpecificationModel? _selectedWheel;

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

        this.CalculatorInput = new GearCalculatorInput(precision: 2);
        this.Name = localizer.GetString("GearCalculator");
        this.AvailableResultUnits = [];
        this.GearCalculResultRows = [];
    }

    public override Task OnInitializedAsync()
    {
        this.FillDefaultValues();
        return base.OnInitializedAsync();
    }

    /// Updates default values with arbitrary values until user configuration management is implemented.
    private void FillDefaultValues()
    {
        this.SelectedCalculatorType = EGearCalculatorType.GearInches;
        this.SourceWheels = new(_repository.GetMostCommonWheelSpecifications());
        this.SelectedWheel = this.SourceWheels.GetMostUsedWheel();
        this.SourceCranks = new(_repository.GetAllCranksetSpecifications());
        this.SelectedCrank = this.SourceCranks.GetMostUsedCrankset();
        this.SourceCadence = new(_repository.GetAllCandences());
        this.SelectedCadence = this.SourceCadence.GetMostUsedCadence();
        this.SourceSprockets = new(_repository.GetMostCommonSprocketSpecifications().Select(x => new SelectibleModel<SprocketSpecificationModel>(x)));
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

        IReadOnlyCollection<Enum> availableUnits = UnitsStore.GetAvailableUnitForGearCalculation(result.Content.UsedInputs.CalculatorType);
        this.AvailableResultUnits = new ObservableCollection<Enum>(availableUnits);
        this.SelectedResultUnit = result.Content.Unit;
    }

    [RelayCommand]
    private void SprocketSelected(SelectibleModel<SprocketSpecificationModel> selectibleSprocket)
    {
        IEnumerable<int> selectedSprockets = this.SourceSprockets
            .Where(x => x.IsSelected)
            .OrderBy(x => x.Value.TeethCount)
            .Select(x => x.Value.TeethCount);

        this.CalculatorInput.WithSprockets(selectedSprockets);
        this.SelectedSprocketsStr = string.Join(", ", selectedSprockets);
        base.OnCalculationInputChanged(nameof(this.CalculatorInput.NumberOfTeethBySprocket));
    }

    partial void OnSelectedCalculatorTypeChanged(EGearCalculatorType value)
    {
        this.CalculatorInput.CalculatorType = value;
        base.OnCalculationInputChanged(nameof(this.CalculatorInput.CalculatorType));
    }

    partial void OnChainring1TeethCountChanged(int? value)
    {
        this.CalculatorInput.TeethNumberLargeOrUniqueChainring = value ?? 0;
        base.OnCalculationInputChanged(nameof(this.CalculatorInput.TeethNumberLargeOrUniqueChainring));
    }

    partial void OnChainring2TeethCountChanged(int? value)
    {
        this.CalculatorInput.TeethNumberMediumChainring = value;
        base.OnCalculationInputChanged(nameof(this.CalculatorInput.TeethNumberMediumChainring));
    }

    partial void OnChainring3TeethCountChanged(int? value)
    {
        this.CalculatorInput.TeethNumberSmallChainring = value;
        base.OnCalculationInputChanged(nameof(this.CalculatorInput.TeethNumberSmallChainring));
    }

    partial void OnSelectedCadenceChanged(CadenceModel value)
    {
        this.CalculatorInput.RevolutionPerMinute = value.Rpm;
        base.OnCalculationInputChanged(nameof(this.CalculatorInput.RevolutionPerMinute));
    }

    partial void OnSelectedWheelChanged(WheelSpecificationModel? value)
    {
        this.CalculatorInput.TyreOuterDiameterIn = value?.TyreOuterDiameterInInch ?? 0;
        base.OnCalculationInputChanged(nameof(this.CalculatorInput.TyreOuterDiameterIn));
    }

    partial void OnSelectedCrankChanged(CranksetSpecificationModel value)
    {
        this.CalculatorInput.CrankLengthMm = value.Length;
        base.OnCalculationInputChanged(nameof(this.CalculatorInput.CrankLengthMm));
    }

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

    [RelayCommand]
    public override void ShowHelpPage()
    {
        base.NavigationService.NavigateToHelpAsync(Enums.EVeloWrenchTools.GearCalculator);
    }

    public override void ResetToDefault()
    {
        base.ResetToDefault();
        this.FillDefaultValues();
    }

    private bool _disposed;
    protected override void Dispose(bool disposing)
    {
        if (disposing && !_disposed)
        {
            this.SourceSprockets?.Clear();
            this.GearCalculResultRows?.Clear();
            this.AvailableResultUnits?.Clear();
            this.SourceWheels?.Clear();
            this.SourceCranks?.Clear();
            this.SourceCadence?.Clear();
            this.SelectedSprocketsStr = null;
            this.SelectedWheel = null;
            this.SelectedResultUnit = null;
            this.SelectedWheel = null;

            _disposed = true;
        }           
        base.Dispose(disposing);
    }
}