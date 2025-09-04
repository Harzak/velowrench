using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.Navigation.Context;
using velowrench.Core.Navigation.Guards;

namespace velowrench.Core.Interfaces;

/// <summary>
/// Interface for navigation guards that control navigation flow
/// </summary>
public interface INavigationGuard
{
    /// <summary>
    /// Determines whether navigation from the specified source view model to the target view model is allowed.
    /// </summary>
    Task<NavigationGuardResult> CanNavigateAsync(IRoutableViewModel? from, IRoutableViewModel to, NavigationContext context);
}
