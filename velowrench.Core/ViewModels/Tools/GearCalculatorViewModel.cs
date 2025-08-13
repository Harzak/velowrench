using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using UnitsNet.Units;
using velowrench.Calculations.Calculs.Transmission.Chain;
using velowrench.Calculations.Calculs.Transmission.Gear;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Interfaces;
using velowrench.Core.Units;
using velowrench.Core.ViewModels.Base;
using velowrench.Repository.Extensions;
using velowrench.Repository.Interfaces;
using velowrench.Repository.Models;
using velowrench.Utils.Enums;
using velowrench.Utils.EventArg;
using velowrench.Utils.Results;

namespace velowrench.Core.ViewModels.Tools;

public sealed partial class GearCalculatorViewModel : BaseRoutableViewModel
{
    private const int PROGRESS_INDICATOR_DELAY = 350;

    private readonly IComponentStandardRepository _repository;
    private readonly ICalcul<GearCalculInput, GearCalculResult> _calcul;
    private OperationResult<GearCalculResult>? _lastResult;

    public override string Name { get; }
    public IEnumerable<EGearCalculType> CalculationTypes => Enum.GetValues<EGearCalculType>();
    public string SelectedSprocketsStr => string.Join(", ", _selectedSprockets.OrderBy(x => x.TeethCount).Select(x => x.Label));

    [ObservableProperty]
    private EGearCalculType _selectedCalculType;

    [ObservableProperty]
    private string? _calculInputErrors;

    [ObservableProperty]
    private ObservableCollection<WheelSpecificationModel> _sourceWheels;

    [ObservableProperty]
    private WheelSpecificationModel _selectedWheel;

    [ObservableProperty]
    private ObservableCollection<CranksetSpecificationModel> _sourceCranks;

    [ObservableProperty]
    private CranksetSpecificationModel _selectedCrank;

    [ObservableProperty]
    private ObservableCollection<CadenceModel> _sourceCadence;

    [ObservableProperty]
    private CadenceModel _selectedCadence;

    [ObservableProperty]
    private ObservableCollection<SprocketSpecificationModel> _sourceSprockets;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedSprocketsStr))]
    private List<SprocketSpecificationModel> _selectedSprockets;

    [ObservableProperty]
    [Range(10, 120)]
    private int _chainring1TeethCount;

    [ObservableProperty]
    [Range(10, 120)]
    private int? _chainring2TeethCount;

    [ObservableProperty]
    [Range(10, 120)]
    private int? _chainring3TeethCount;

    /// <summary>
    /// Gets or sets the current state of the chain length calculation.
    /// </summary>
    [ObservableProperty]
    private ECalculState _currentState;

    [ObservableProperty]
    ObservableCollection<DataModel> _datas;

    public GearCalculatorViewModel(
        ICalculFactory<GearCalculInput, GearCalculResult> calculFactory,
        INavigationService navigationService,
        IComponentStandardRepository repository,
        ILocalizer localizer) : base(navigationService)
    {
        ArgumentNullException.ThrowIfNull(calculFactory, nameof(calculFactory));
        ArgumentNullException.ThrowIfNull(navigationService, nameof(navigationService));
        ArgumentNullException.ThrowIfNull(localizer, nameof(localizer));
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));

        _repository = repository;
        _calcul = calculFactory.CreateCalcul();
        _calcul.StateChanged += OnCalculStateChanged;

        this.Name = localizer.GetString("GearCalculator");
        this.CurrentState = ECalculState.NotStarted;

        _selectedCalculType = EGearCalculType.GearInches;
        _sourceWheels = new(_repository.GetMostCommonWheelSpecifications());
        _selectedWheel = this.SourceWheels.GetMostUsedWheel();
        _sourceCranks = new(_repository.GetAllCranksetSpecifications());
        _selectedCrank = this.SourceCranks.GetMostUsedCrankset();
        _sourceCadence = new(_repository.GetAllCandences());
        _selectedCadence = this.SourceCadence.GetMostUsedCadence();
        _sourceSprockets = new(_repository.GetMostCommonSprocketSpecifications());
        _selectedSprockets = [];
        _datas = [];
        _chainring1TeethCount = 10;
        _chainring1TeethCount = 20;
        _chainring1TeethCount = 30;
        _selectedCadence = this.SourceCadence.First();
        _selectedWheel = this.SourceWheels.First();
        _selectedCrank = this.SourceCranks.First();
    }

    partial void OnSelectedCalculTypeChanged(EGearCalculType value)
    {
        this.OnInputsChanged();
    }

    partial void OnChainring1TeethCountChanged(int value)
    {
        this.OnInputsChanged();
    }

    partial void OnChainring2TeethCountChanged(int? value)
    {
        this.OnInputsChanged();
    }

    partial void OnChainring3TeethCountChanged(int? value)
    {
        this.OnInputsChanged();
    }

    partial void OnSelectedCadenceChanged(CadenceModel value)
    {
        this.OnInputsChanged();
    }

    partial void OnSelectedWheelChanging(WheelSpecificationModel value)
    {
        this.OnInputsChanged();
    }

    partial void OnSelectedCrankChanged(CranksetSpecificationModel value)
    {
        this.OnInputsChanged();
    }

    [RelayCommand]
    private void SprocketSelected(SprocketSpecificationModel sprocket)
    {
        if (sprocket == null)
        {
            return;
        }
        if (!this.SelectedSprockets.Remove(sprocket))
        {
            this.SelectedSprockets.Add(sprocket);
        }
        OnPropertyChanged(nameof(SelectedSprocketsStr));
        this.OnInputsChanged();
    }

    /// <summary>
    /// Handles input changes and triggers calculation if inputs are valid.
    /// </summary>
    private void OnInputsChanged()
    {
        GearCalculInput input = new()
        {
            CalculType = this.SelectedCalculType,
            TeethNumberLargeOrUniqueChainring = this.Chainring1TeethCount,
            TeethNumberMediumChainring = this.Chainring2TeethCount,
            TeethNumberSmallChainring = this.Chainring3TeethCount,
            NumberOfTeethBySprocket = [.. this.SelectedSprockets.OrderBy(x => x.TeethCount).Select(x => x.TeethCount)],
            WheelDiameterInInch = this.SelectedWheel.BSDin,
            CrankLengthInInch = this.SelectedCrank.Length,
            RevolutionPerMinute = this.SelectedCadence.Rpm,
            Precision = 2
        };

        if (this.CanStartCalculation(input))
        {
            this.StartCalculation(input);
        }
        else if (!this.InputsAreValid(input))
        {
            this.CurrentState = ECalculState.NotStarted;
        }
    }

    /// <summary>
    /// Validates that all required inputs have valid values.
    /// </summary>
    private bool InputsAreValid(GearCalculInput input)
    {
        ICalculInputValidation<GearCalculInput> validation = _calcul.GetValidation();
        if (!validation.Validate(input))
        {
            this.CalculInputErrors = string.Join(Environment.NewLine, validation.ErrorMessages);
            return false;
        }
        return true;
    }

    private async void OnCalculStateChanged(object? sender, CalculStateEventArgs e)
    {
        if (this.CurrentState == ECalculState.InProgress && e.State == ECalculState.Computed)
        {
            await Task.Delay(PROGRESS_INDICATOR_DELAY).ConfigureAwait(false);
        }
        this.CurrentState = e.State;
    }

    /// <summary>
    /// Determines whether a calculation can be started based on current input validity and calculation state.
    /// </summary>
    private bool CanStartCalculation(GearCalculInput input)
    {
        return !base.HasErrors
            && this.InputsAreValid(input)
            && _calcul.State != ECalculState.InProgress
            && (_lastResult?.Content?.UsedInputs == null || _lastResult.Content.UsedInputs != input);
    }

    private void StartCalculation(GearCalculInput input)
    {
        _lastResult = _calcul.Start(input);

        if (_lastResult.IsSuccess && _lastResult.HasContent)
        {
            this.Datas.Clear();
            for (int i = 0; i < _lastResult.Content.ValuesLargeOrUniqueChainring.Count; i++)
            {
                this.Datas.Add(new DataModel()
                {
                    Chainring1 = _lastResult.Content.ValuesLargeOrUniqueChainring[i],
                    Chainring2 = _lastResult.Content.ValuesMediumChainring?.Count > i ? _lastResult.Content.ValuesMediumChainring[i] : null,
                    Chainring3 = _lastResult.Content.ValuesSmallChainring?.Count > i ? _lastResult.Content.ValuesSmallChainring[i] : null,
                    SprocketCount = _lastResult.Content.UsedInputs.NumberOfTeethBySprocket[i]
                });
            }
        }
    }

}

public class DataModel
{
    public int SprocketCount { get; set; }
    public double Chainring1 { get; set; }
    public double? Chainring2 { get; set; }
    public double? Chainring3 { get; set; }
}