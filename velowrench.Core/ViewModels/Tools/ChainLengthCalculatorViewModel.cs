using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using UnitsNet.Units;
using velowrench.Calculations.Calculs.Transmission.ChainLength;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Interfaces;
using velowrench.Core.Units;
using velowrench.Core.ViewModels.Base;
using velowrench.Utils.Enums;
using velowrench.Utils.EventArg;
using velowrench.Utils.Results;

namespace velowrench.Core.ViewModels.Tools;

public partial class ChainLengthCalculatorViewModel : BaseRoutableViewModel
{
    private const int PROGRESS_INDICATOR_DELAY = 350;

    private readonly ICalcul<ChainLengthCalculInput, ChainLengthCalculResult> _calcul;
    private OperationResult<ChainLengthCalculResult>? _lastResult;

    public override string Name { get; }
    public override bool CanShowHelpPage => true;

    [ObservableProperty]
    private ConvertibleDouble<LengthUnit>? _chainStayLength;

    [ObservableProperty]
    [Range(1, 100, ErrorMessage = "Must be between 1 and 100 teeth")]
    [NotifyDataErrorInfo]
    private int? _teethLargestChainring;

    [ObservableProperty]
    [Range(1, 70, ErrorMessage = "Must be between 1 and 70 teeth")]
    [NotifyDataErrorInfo]
    private int? _teethLargestSprocket;

    [ObservableProperty]
    private int _recommendedChainLinks;

    [ObservableProperty]
    private ConvertibleDouble<LengthUnit>? _recommendedChainLength;

    [ObservableProperty]
    private ECalculState _currentState;

    public ChainLengthCalculatorViewModel(ICalculFactory<ChainLengthCalculInput, ChainLengthCalculResult> calculFactory,
        INavigationService navigationService,
        ILocalizer localizer)
    : base(navigationService)
    {
        ArgumentNullException.ThrowIfNull(calculFactory, nameof(calculFactory));

        _calcul = calculFactory.CreateCalcul();
        _calcul.StateChanged += OnChainLengthCalculStateChanged;
        _chainStayLength = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Centimeter);
        _chainStayLength.ValueChanged += this.OnChainStayLengthValueChanged;

        this.Name = localizer.GetString("ChainLengthCalculator");
        this.CurrentState = ECalculState.NotStarted; ;
    }

    private void OnChainStayLengthValueChanged(object? sender, EventArgs e)
    {
        this.OnInputsChanged();
    }

    partial void OnTeethLargestChainringChanged(int? value)
    {
        this.OnInputsChanged();
    }

    partial void OnTeethLargestSprocketChanged(int? value)
    {
        this.OnInputsChanged();
    }

    private void OnInputsChanged()
    {
        if (this.CanStartCalculation())
        {
            this.StartCalculation();
        }
        else if (!this.InputsAreValid())
        {
            this.CurrentState = ECalculState.NotStarted;
        }
    }

    private bool CanStartCalculation()
    {
        return !base.HasErrors && this.InputsAreValid() && _calcul.State != ECalculState.InProgress;
    }

    private bool InputsAreValid()
    {
        return _chainStayLength?.Value > 0
                && _teethLargestChainring.HasValue  && _teethLargestChainring > 0
                && _teethLargestSprocket.HasValue && _teethLargestSprocket > 0;
    }

    private async void OnChainLengthCalculStateChanged(object? sender, CalculStateEventArgs e)
    {
        if (this.CurrentState == ECalculState.InProgress && e.State == ECalculState.Computed)
        {
            await Task.Delay(PROGRESS_INDICATOR_DELAY);
        }
        this.CurrentState = e.State;
    }

    private void StartCalculation()
    {
        if (base.HasErrors || _calcul.State == ECalculState.InProgress)
        {
            return;
        }

        ChainLengthCalculInput input = new()
        {
            ChainStayLengthInch = this.ChainStayLength?.GetValueIn(LengthUnit.Inch) ?? 0,
            TeethLargestChainring = this.TeethLargestChainring ?? 0,
            TeethLargestSprocket = this.TeethLargestSprocket ?? 0
        };

        if (_lastResult?.Content?.UsedInputs != null && _lastResult.Content.UsedInputs == input)
        {
            return;
        }

        _lastResult = _calcul.Start(input);

        if (_lastResult.IsSuccess && _lastResult.HasContent)
        {
            this.RecommendedChainLength = new ConvertibleDouble<LengthUnit>(_lastResult.Content.ChainLengthInch, LengthUnit.Inch);
            this.RecommendedChainLinks = _lastResult.Content.ChainLinks;
        }
    }

    public override void ShowHelpPage()
    {
        base.NavigationService.NavigateToHelp(Enums.EVeloWrenchTools.ChainLengthCalculator);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_calcul != null)
            {
                _calcul.StateChanged -= OnChainLengthCalculStateChanged;
            }
            if (_chainStayLength != null)
            {
                _chainStayLength.ValueChanged -= this.OnChainStayLengthValueChanged;
            }
        }
        base.Dispose(disposing);
    }
}