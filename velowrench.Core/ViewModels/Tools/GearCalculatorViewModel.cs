using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using UnitsNet.Units;
using velowrench.Calculations.Calculs.Transmission.Chain;
using velowrench.Calculations.Calculs.Transmission.Gear;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Interfaces;
using velowrench.Core.Models;
using velowrench.Core.Units;
using velowrench.Core.ViewModels.Base;
using velowrench.Repository.Extensions;
using velowrench.Repository.Interfaces;
using velowrench.Repository.Models;
using velowrench.Utils.Enums;
using velowrench.Utils.EventArg;
using velowrench.Utils.Results;

namespace velowrench.Core.ViewModels.Tools;

/// <summary>
/// View model for the gear calculator tool that performs bicycle gear ratio calculations.
/// Provides gear analysis including gear inches, development, gain ratio, and speed calculations
/// for various chainring and sprocket combinations.
/// </summary>
public sealed partial class GearCalculatorViewModel : BaseCalculViewModel<GearCalculInput, GearCalculResult>
{
    private readonly IComponentStandardRepository _repository;

    /// <summary>
    /// Gets the display name of this view model.
    /// </summary>
    public override string Name { get; }

    /// <summary>
    /// Gets all available gear calculation types for user selection.
    /// </summary>
    public IEnumerable<EGearCalculType> CalculationTypes => Enum.GetValues<EGearCalculType>();

    /// <summary>
    /// Gets a formatted string representation of currently selected sprockets.
    /// Used for UI display and user feedback about current sprocket selection.
    /// </summary>
    public string SelectedSprocketsStr => string.Join(", ", SourceSprockets.Where(x => x.IsSelected).OrderBy(x => x.Value.TeethCount).Select(x => x.Value.Label));

    /// <summary>
    /// Gets or sets the type of gear calculation to perform.
    /// Determines which calculation algorithm is used and which optional parameters are required.
    /// </summary>
    [ObservableProperty]
    private EGearCalculType _selectedCalculType;

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
    /// Required when calculation type is <see cref="EGearCalculType.GainRatio"/>.
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
    /// Required when calculation type is <see cref="EGearCalculType.Speed"/>.
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
    private int _chainring1TeethCount;

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
    /// Gets or sets the collection of calculated gear data for display.
    /// </summary>
    [ObservableProperty]
    ObservableCollection<GearCalculResultRowModel> _gearCalculResultRows;

    public GearCalculatorViewModel(
        ICalculFactory<GearCalculInput, GearCalculResult> calculFactory,
        INavigationService navigationService,
        IComponentStandardRepository repository,
        ILocalizer localizer)
    : base(calculFactory, navigationService)
    {
        ArgumentNullException.ThrowIfNull(localizer, nameof(localizer));
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));

        _repository = repository;

        _selectedCalculType = EGearCalculType.GearInches;
        _sourceWheels = new(_repository.GetMostCommonWheelSpecifications());
        _selectedWheel = this.SourceWheels.GetMostUsedWheel();
        _sourceCranks = new(_repository.GetAllCranksetSpecifications());
        _selectedCrank = this.SourceCranks.GetMostUsedCrankset();
        _sourceCadence = new(_repository.GetAllCandences());
        _selectedCadence = this.SourceCadence.GetMostUsedCadence();
        _sourceSprockets = new(_repository.GetMostCommonSprocketSpecifications().Select(x => new SelectibleModel<SprocketSpecificationModel>(x)));
        _gearCalculResultRows = [];
        _chainring1TeethCount = 10;
        _chainring1TeethCount = 20;
        _chainring1TeethCount = 30;
        _selectedCadence = this.SourceCadence.First();
        _selectedWheel = this.SourceWheels.First();
        _selectedCrank = this.SourceCranks.First();

        this.Name = localizer.GetString("GearCalculator");
    }

    /// <summary>
    /// Handles sprocket selection changes in the user interface.
    /// </summary>
    [RelayCommand]
    private void SprocketSelected(SelectibleModel<SprocketSpecificationModel> selectibleSprocket)
    {
        OnPropertyChanged(nameof(SelectedSprocketsStr));
        base.RefreshCalculation();
    }

    /// <summary>
    /// Creates calculation input based on current view model state.
    /// Maps UI properties to the gear calculation input structure required by the calculation engine.
    /// </summary>
    protected override GearCalculInput GetInput()
    {
        return new GearCalculInput()
        {
            CalculType = this.SelectedCalculType,
            TeethNumberLargeOrUniqueChainring = this.Chainring1TeethCount,
            TeethNumberMediumChainring = this.Chainring2TeethCount,
            TeethNumberSmallChainring = this.Chainring3TeethCount,
            NumberOfTeethBySprocket = [.. this.SourceSprockets.Where(x => x.IsSelected).OrderBy(x => x.Value.TeethCount).Select(x => x.Value.TeethCount)],
            WheelDiameterInInch = this.SelectedWheel.BSDin,
            CrankLengthInInch = this.SelectedCrank.Length,
            RevolutionPerMinute = this.SelectedCadence.Rpm,
            Precision = 2
        };
    }

    /// <summary>
    /// Processes successful gear calculation results and updates the display data.
    /// </summary>
    protected override void OnCalculSuccessfull(OperationResult<GearCalculResult> result)
    {
        this.GearCalculResultRows.Clear();
        for (int i = 0; i < result.Content.ValuesLargeOrUniqueChainring.Count; i++)
        {
            this.GearCalculResultRows.Add(new GearCalculResultRowModel()
            {
                Chainring1 = result.Content.ValuesLargeOrUniqueChainring[i],
                Chainring2 = result.Content.ValuesMediumChainring?.Count > i ? result.Content.ValuesMediumChainring[i] : null,
                Chainring3 = result.Content.ValuesSmallChainring?.Count > i ? result.Content.ValuesSmallChainring[i] : null,
                SprocketCount = result.Content.UsedInputs.NumberOfTeethBySprocket[i]
            });
        }
    }

    /// <summary>
    /// Handles changes to the calculation type selection.
    /// Triggers input validation and potential recalculation when the calculation method changes.
    /// </summary>
    partial void OnSelectedCalculTypeChanged(EGearCalculType value)  {
        base.RefreshCalculation();
    }

    /// <summary>
    /// Handles changes to the primary chainring teeth count.
    /// </summary>
    partial void OnChainring1TeethCountChanged(int value)
    {
        base.RefreshCalculation();
    }

    /// <summary>
    /// Handles changes to the medium chainring teeth count.
    /// </summary>
    partial void OnChainring2TeethCountChanged(int? value)
    {
        base.RefreshCalculation();
    }

    /// <summary>
    /// Handles changes to the smallest chainring teeth count.
    /// </summary>
    partial void OnChainring3TeethCountChanged(int? value)
    {
        base.RefreshCalculation();
    }

    /// <summary>
    /// Handles changes to the selected cadence value.
    /// </summary>
    partial void OnSelectedCadenceChanged(CadenceModel value)
    {
        base.RefreshCalculation();
    }

    /// <summary>
    /// Handles changes to the selected wheel specification.
    /// </summary>
    partial void OnSelectedWheelChanging(WheelSpecificationModel value)
    {
        base.RefreshCalculation();
    }

    /// <summary>
    /// Handles changes to the selected crankset specification.
    /// </summary>
    partial void OnSelectedCrankChanged(CranksetSpecificationModel value)
    {
        base.RefreshCalculation();
    }
}

/// <summary>
/// Represents a single row of gear calculation results for display purposes.
/// Contains calculated values for a specific sprocket size across all available chainrings.
/// </summary>
public class GearCalculResultRowModel
{
    /// <summary>
    /// Gets or sets the number of teeth on the sprocket for this calculation row.
    /// This value identifies which sprocket size this row represents.
    /// </summary>
    public int SprocketCount { get; set; }
    
    /// <summary>
    /// Gets or sets the calculated value for the primary (largest or only) chainring.
    /// Always contains a value as the primary chainring is required for all calculations.
    /// </summary>
    public double Chainring1 { get; set; }
    
    /// <summary>
    /// Gets or sets the calculated value for the medium chainring.
    /// Contains a value only when a medium chainring is configured in the input.
    /// </summary>
    public double? Chainring2 { get; set; }
    
    /// <summary>
    /// Gets or sets the calculated value for the smallest chainring.
    /// Contains a value only when a small chainring is configured in the input.
    /// </summary>
    public double? Chainring3 { get; set; }
}