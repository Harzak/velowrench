using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Core.ViewModels.Home;

public interface IToolbar
{
    public bool CanShowHelpPage { get; }
    public bool CanShowContextMenu { get; }
    public bool CanShowProfile { get; }

    public Action? ShowHelpPageAction { get; set; }
    public Action? ResetToDefaultAction { get; set; }
    public Action? ShowProfilAction { get; set; }
}


public  sealed partial class ToolbarViewModel : ObservableObject, IToolbar
{
    private Action? _showHelpPageAction;
    private Action? _resetToDefaultAction;
    private Action? _showProfilAction;

    public Action? ShowHelpPageAction
    {
        get => _showHelpPageAction;
        set
        {
            _showHelpPageAction = value;
            OnPropertyChanged(nameof(CanShowHelpPage));
        }
    }

    public Action? ResetToDefaultAction
    {
        get => _resetToDefaultAction;
        set
        {
            _resetToDefaultAction = value;
            OnPropertyChanged(nameof(CanShowContextMenu));
        }
    }

    public Action? ShowProfilAction
    {
        get => _showProfilAction;
        set
        {
            _showProfilAction = value;
            OnPropertyChanged(nameof(CanShowProfile));
        }
    }

    public bool CanShowHelpPage => _showHelpPageAction != null;

    public bool CanShowContextMenu => _resetToDefaultAction != null;

    public bool CanShowProfile => _showProfilAction != null;

    [RelayCommand]
    private void ShowHelpPage()
    {
        _showHelpPageAction?.Invoke();
    }

    [RelayCommand]
    private void ResetToDefault()
    {
        _resetToDefaultAction?.Invoke();
    }

    [RelayCommand]
    private void ShowProfile()
    {
        _showProfilAction?.Invoke();
    }

    public void Reset()
    {
        _showHelpPageAction = null;
        _resetToDefaultAction = null;
        _showProfilAction = null;
        OnPropertyChanged(nameof(CanShowHelpPage));
        OnPropertyChanged(nameof(CanShowContextMenu));
        OnPropertyChanged(nameof(CanShowProfile));
    }
}
