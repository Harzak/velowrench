using velowrench.Core.Enums;
using velowrench.Core.EventArg;

namespace velowrench.Core.Interfaces;
public interface INavigationService
{
    void NavigateToHome();
    void NavigateToTool(EVeloWrenchTools toolType);
    void NavigateToHelp(EVeloWrenchTools toolType);
    void NavigateBack();
    bool CanNavigateBack { get; }
    IRoutableViewModel? CurrentViewModel { get; }

    event EventHandler<ViewModelChangedEventArgs> CurrentViewModelChanged;
    void ClearNavigationStack();
}