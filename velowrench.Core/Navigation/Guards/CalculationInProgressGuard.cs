using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculators;
using velowrench.Core.Interfaces;
using velowrench.Core.Navigation.Context;
using velowrench.Core.ViewModels.Base;
using velowrench.Utils.Enums;

namespace velowrench.Core.Navigation.Guards;

/// <summary>
/// Guard that prevents navigation when calculations are in progress
/// </summary>
public sealed class CalculationInProgressGuard : INavigationGuard
{
    public Task<NavigationGuardResult> CanNavigateAsync(IRoutableViewModel? from, IRoutableViewModel to, NavigationContext context)
    {
        //if (from is BaseCalculatorViewModel calculatorViewModel)
        //{
        //    if (calculatorViewModel.CurrentState == ECalculatorState.InProgress)
        //    {
        //        return NavigationGuardResult.Deny("Cannot navigate while calculation is in progress");
        //    }
        //}

        return Task.FromResult(NavigationGuardResult.Allow());
    }
}
