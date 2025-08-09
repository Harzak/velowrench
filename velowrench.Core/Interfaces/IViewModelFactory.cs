using velowrench.Core.Enums;

namespace velowrench.Core.Interfaces;

public interface IViewModelFactory
{
    IRoutableViewModel CreateToolViewModel(EVeloWrenchTools type, INavigationService navigationService);
    IRoutableViewModel CreateHelpViewModel(EVeloWrenchTools type, INavigationService navigationService);
    IRoutableViewModel CreateHomeViewModel(INavigationService navigationService);
}