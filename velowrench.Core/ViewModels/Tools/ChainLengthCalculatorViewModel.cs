using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using velowrench.Calculations.Calculs.Transmission.ChainLength;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;
using velowrench.Utils.Results;

namespace velowrench.Core.ViewModels.Tools;

public partial class ChainLengthCalculatorViewModel : BaseRoutableViewModel
{
    private readonly ICalcul<ChainLengthCalculInput, ChainLengthCalculResult> _calcul;

    public override string Name { get; }

    [ObservableProperty]
    [Range(0.1, 1000.0, ErrorMessage = "Chain stay length must be between 0.1 and 1000.0 mm")]
    [NotifyDataErrorInfo]
    private double? _chainStayLength;

    [ObservableProperty]
    [Range(1, 300, ErrorMessage = "Largest chainring must be between 1 and 300 teeth")]
    [NotifyDataErrorInfo]
    private int? _largestChainring;

    [ObservableProperty]
    [Range(1, 100, ErrorMessage = "Largest sprocket must be between 1 and 100 teeth")]
    [NotifyDataErrorInfo]
    private int? _largestSprocket;

    [ObservableProperty]
    private int _chainLengthInLinks;

    [ObservableProperty]
    private double _chainLengthInInch;

    public ChainLengthCalculatorViewModel(ICalculFactory<ChainLengthCalculInput, ChainLengthCalculResult> calculFactory,
        INavigationService navigationService,
        ILocalizer localizer)
    : base(navigationService)
    {
        ArgumentNullException.ThrowIfNull(calculFactory, nameof(calculFactory));

        _calcul = calculFactory.CreateCalcul();
        this.Name = localizer.GetString("ChainLengthCalculator");
        this.InitializeDefaultValues();
    }

    private void InitializeDefaultValues()
    {
        _chainStayLength = 400;
        _largestChainring = 50;
        _largestSprocket = 28;
        this.Calculate();
    }

    partial void OnChainStayLengthChanged(double? value)
    {
        if (value.HasValue && value > 0)
        {
            Calculate();
        }
    }

    partial void OnLargestChainringChanged(int? value)
    {
        if (value.HasValue && value > 0)
        {
            Calculate();
        }
    }

    partial void OnLargestSprocketChanged(int? value)
    {
        if (value.HasValue && value > 0)
        {
            Calculate();
        }
    }

    private void Calculate()
    {
        if (base.HasErrors)
        {
            return;
        }

        ChainLengthCalculInput input = new()
        {
            ChainStayLength = this.ChainStayLength ?? 0,
            LargestChainring = this.LargestChainring ?? 0,
            LargestSprocket = this.LargestSprocket ?? 0
        };
        OperationResult<ChainLengthCalculResult> calculResult = _calcul.Start(input);
        if (calculResult.IsSuccess && calculResult.HasContent)
        {
            this.ChainLengthInInch = calculResult.Content.ChainLengthInInches;
        }
    }
}