using Avalonia.Platform;
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
    private readonly ICalcul<ChainLengthCalculInput, ChainLengthCalculResult> _calcul;

    public override string Name { get; }

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
    private int _recomendedChainLinks;

    [ObservableProperty]
    private ConvertibleDouble<LengthUnit>? _recomendedChainLength;

    [ObservableProperty]
    private ECalculState _calculState;

    public ChainLengthCalculatorViewModel(ICalculFactory<ChainLengthCalculInput, ChainLengthCalculResult> calculFactory,
        INavigationService navigationService,
        ILocalizer localizer)
    : base(navigationService)
    {
        ArgumentNullException.ThrowIfNull(calculFactory, nameof(calculFactory));

        _calcul = calculFactory.CreateCalcul();
        _calcul.StateChanged += OnChainLengthCalculStateChanged;
        this.CalculState = _calcul.State;
        this.Name = localizer.GetString("ChainLengthCalculator");

        this.InitializeDefaultValues();
    }

    private void InitializeDefaultValues()
    {
        _chainStayLength = new ConvertibleDouble<LengthUnit>(40, LengthUnit.Centimeter);
        _teethLargestChainring = 50;
        _teethLargestSprocket = 28;

        _chainStayLength.ValueChanged += OnChainStayLengthValueChanged;
        CalculState = ECalculState.NotStarted;
        //this.Calculate();
    }

    private void OnChainStayLengthValueChanged(object? sender, EventArgs e)
    {
        if (ChainStayLength?.Value > 0)
        {
            Calculate();
        }
    }

    partial void OnTeethLargestChainringChanged(int? value)
    {
        if (value.HasValue && value > 0)
        {
            Calculate();
        }
    }

    partial void OnTeethLargestSprocketChanged(int? value)
    {
        if (value.HasValue && value > 0)
        {
            Calculate();
        }
    }

    private void OnChainLengthCalculStateChanged(object? sender, CalculStateEventArgs e)
    {
        this.CalculState = e.State;
    }

    private void Calculate()
    {
        if (base.HasErrors || _calcul.State == Utils.Enums.ECalculState.InProgress)
        {
            return;
        }

        ChainLengthCalculInput input = new()
        {
            ChainStayLengthInch = this.ChainStayLength?.GetValueIn(LengthUnit.Inch) ?? 0,
            TeethLargestChainring = this.TeethLargestChainring ?? 0,
            TeethLargestSprocket = this.TeethLargestSprocket ?? 0
        };

        OperationResult<ChainLengthCalculResult> calculResult = _calcul.Start(input);

        if (calculResult.IsSuccess && calculResult.HasContent)
        {
            this.RecomendedChainLength = new ConvertibleDouble<LengthUnit>(calculResult.Content.ChainLengthInch, LengthUnit.Inch);
            this.RecomendedChainLinks = calculResult.Content.ChainLinks;
        }
    }
}