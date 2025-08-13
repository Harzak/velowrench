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

public sealed partial class GearCalculatorViewModel : BaseCalculViewModel<GearCalculInput, GearCalculResult>
{
    private readonly IComponentStandardRepository _repository;

    public override string Name { get; }
    public IEnumerable<EGearCalculType> CalculationTypes => Enum.GetValues<EGearCalculType>();
    public string SelectedSprocketsStr => string.Join(", ", SourceSprockets.Where(x => x.IsSelected).OrderBy(x => x.Value.TeethCount).Select(x => x.Value.Label));

    [ObservableProperty]
    private EGearCalculType _selectedCalculType;

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
    private ObservableCollection<SelectibleModel<SprocketSpecificationModel>> _sourceSprockets;

    [ObservableProperty]
    [Range(10, 120)]
    private int _chainring1TeethCount;

    [ObservableProperty]
    [Range(10, 120)]
    private int? _chainring2TeethCount;

    [ObservableProperty]
    [Range(10, 120)]
    private int? _chainring3TeethCount;

    [ObservableProperty]
    ObservableCollection<DataModel> _datas;

    public GearCalculatorViewModel(
        ICalculFactory<GearCalculInput, GearCalculResult> calculFactory,
        INavigationService navigationService,
        IComponentStandardRepository repository,
        ILocalizer localizer)
    : base(calculFactory, navigationService)
    {
        ArgumentNullException.ThrowIfNull(calculFactory, nameof(calculFactory));
        ArgumentNullException.ThrowIfNull(navigationService, nameof(navigationService));
        ArgumentNullException.ThrowIfNull(localizer, nameof(localizer));
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));

        _repository = repository;

        this.Name = localizer.GetString("GearCalculator");

        _selectedCalculType = EGearCalculType.GearInches;
        _sourceWheels = new(_repository.GetMostCommonWheelSpecifications());
        _selectedWheel = this.SourceWheels.GetMostUsedWheel();
        _sourceCranks = new(_repository.GetAllCranksetSpecifications());
        _selectedCrank = this.SourceCranks.GetMostUsedCrankset();
        _sourceCadence = new(_repository.GetAllCandences());
        _selectedCadence = this.SourceCadence.GetMostUsedCadence();
        _sourceSprockets = new(_repository.GetMostCommonSprocketSpecifications().Select(x => new SelectibleModel<SprocketSpecificationModel>(x)));
        _datas = [];
        _chainring1TeethCount = 10;
        _chainring1TeethCount = 20;
        _chainring1TeethCount = 30;
        _selectedCadence = this.SourceCadence.First();
        _selectedWheel = this.SourceWheels.First();
        _selectedCrank = this.SourceCranks.First();
    }

    [RelayCommand]
    private void SprocketSelected(SelectibleModel<SprocketSpecificationModel> selectibleSprocket)
    {
        OnPropertyChanged(nameof(SelectedSprocketsStr));
        base.OnInputsChanged();
    }

    partial void OnSelectedCalculTypeChanged(EGearCalculType value)
    {
        base.OnInputsChanged();
    }

    partial void OnChainring1TeethCountChanged(int value)
    {
        base.OnInputsChanged();
    }

    partial void OnChainring2TeethCountChanged(int? value)
    {
        base.OnInputsChanged();
    }

    partial void OnChainring3TeethCountChanged(int? value)
    {
        base.OnInputsChanged();
    }

    partial void OnSelectedCadenceChanged(CadenceModel value)
    {
        base.OnInputsChanged();
    }

    partial void OnSelectedWheelChanging(WheelSpecificationModel value)
    {
        base.OnInputsChanged();
    }

    partial void OnSelectedCrankChanged(CranksetSpecificationModel value)
    {
        base.OnInputsChanged();
    }

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

    protected override void OnCalculSuccessfull(OperationResult<GearCalculResult> result)
    {
        this.Datas.Clear();
        for (int i = 0; i < result.Content.ValuesLargeOrUniqueChainring.Count; i++)
        {
            this.Datas.Add(new DataModel()
            {
                Chainring1 = result.Content.ValuesLargeOrUniqueChainring[i],
                Chainring2 = result.Content.ValuesMediumChainring?.Count > i ? result.Content.ValuesMediumChainring[i] : null,
                Chainring3 = result.Content.ValuesSmallChainring?.Count > i ? result.Content.ValuesSmallChainring[i] : null,
                SprocketCount = result.Content.UsedInputs.NumberOfTeethBySprocket[i]
            });
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