using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculs;
using velowrench.Calculations.Calculs.Transmission.Gear;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Tools;
using velowrench.Utils.Enums;
using velowrench.Utils.EventArg;
using velowrench.Utils.Results;

namespace velowrench.Core.ViewModels.Base;

/// <summary>
/// Provides a base implementation for view models that perform calculations with input validation,
/// state management, and result handling. This abstract class standardizes the calculation workflow
/// across different calculation types while allowing specific implementations for input creation and result processing.
/// </summary>
public abstract partial class BaseCalculViewModel<TInput, TResult> : BaseRoutableViewModel 
    where TInput : class 
    where TResult : BaseCalculResult<TInput>
{
    private const int PROGRESS_INDICATOR_DELAY = 350;

    private OperationResult<TResult>? _lastResult;

    /// <summary>
    /// Gets the calculation engine instance used to perform calculations.
    /// This instance handles the actual calculation logic, validation, and state management.
    /// </summary>
    protected ICalcul<TInput, TResult> Calcul { get; }

    /// <summary>
    /// Gets or sets the current state of the calculation operation.
    /// </summary>
    [ObservableProperty]
    private ECalculState _currentState;

    /// <summary>
    /// Gets or sets error messages from input validation failures.
    /// </summary>
    [ObservableProperty]
    private string? _calculInputErrors;

    protected BaseCalculViewModel(ICalculFactory<TInput, TResult> calculFactory, INavigationService navigationService) : base(navigationService)
    {
        this.Calcul = calculFactory?.CreateCalcul() ?? throw new ArgumentNullException(nameof(calculFactory));
        this.Calcul.StateChanged += OnCalculStateChanged;
        this.CurrentState = ECalculState.NotStarted;
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
    /// Handles input changes and triggers calculation if inputs are valid.
    /// </summary>
    protected void RefreshCalculation()
    {
        TInput input = this.GetInput();
        if (this.CanStartCalculation(input))
        {
            this.StartCalculation(input);
        }
    }

    /// <summary>
    /// Determines whether a calculation can be started based on current conditions.
    /// </summary>
    protected virtual bool CanStartCalculation(TInput input)
    {
        return !base.HasErrors
            && this.InpuValidation(input)
            && this.Calcul.State != ECalculState.InProgress
            && (_lastResult == null || !_lastResult.Content.UsedInputs.Equals(input));
    }

    private void StartCalculation(TInput input)
    {
        _lastResult = this.Calcul.Start(input);

        if (_lastResult.IsSuccess && _lastResult.HasContent)
        {
            this.OnCalculSuccessfull(_lastResult);
        }
    }

    /// <summary>
    /// Creates and returns the calculation input based on current view model state.
    /// </summary>
    protected abstract TInput GetInput();

    /// <summary>
    /// Processes successful calculation results and updates the view model state.
    /// </summary>
    protected abstract void OnCalculSuccessfull(OperationResult<TResult> result);

    /// <summary>
    /// Validates that all required inputs have valid values.
    /// </summary>
    private bool InpuValidation(TInput input)
    {
        ICalculInputValidation<TInput> validation = this.Calcul.GetValidation();
        if (!validation.Validate(input))
        {
            this.CalculInputErrors = string.Join(Environment.NewLine, validation.ErrorMessages);
            this.CurrentState = ECalculState.NotStarted;
            return false;
        }
        return true;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (this.Calcul != null)
            {
                this.Calcul.StateChanged -= OnCalculStateChanged;
            }
        }
        base.Dispose(disposing);
    }
}
