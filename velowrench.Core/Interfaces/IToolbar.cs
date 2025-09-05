namespace velowrench.Core.Interfaces;

public interface IToolbar
{
    public bool CanShowHelpPage { get; }
    public bool CanShowContextMenu { get; }
    public bool CanShowProfile { get; }

    public Action? ShowHelpPageAction { get; set; }
    public Action? ResetToDefaultAction { get; set; }
    public Action? ShowProfilAction { get; set; }
}