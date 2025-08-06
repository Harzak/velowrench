using velowrench.Core.Enums;
using velowrench.Core.Services;

namespace velowrench.Core.Interfaces;

public interface IViewModelFactory
{
   IRoutableViewModel CreateToolViewModel(EVeloWrenchTools type, INavigationService navigationService);
   IRoutableViewModel CreateHomeViewModel(INavigationService navigationService);
}