﻿namespace velowrench.Core.Interfaces;

/// <summary>
/// Interface for view models that can be navigated to and managed by the navigation service.
/// </summary>
public interface IRoutableViewModel : IDisposable
{
    /// <summary>
    /// Gets the display name of the view model for UI presentation.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the URL path segment identifier for this view model.
    /// </summary>
    public string UrlPathSegment { get; }

    /// <summary>
    /// Gets a value indicating whether this view model has an associated help page.
    /// </summary>
    public bool CanShowHelpPage { get; }

    /// <summary>
    /// Gets a value indicating whether the context (pop-up) menu can be displayed.
    /// </summary>
    public bool CanShowContextMenu { get; }

    /// <summary>
    /// Asynchronously performs initialization logic when the component is first rendered.
    /// </summary>
    Task OnInitializedAsync();

    /// <summary>
    /// Performs asynchronous cleanup operations when the object is being destroyed by the navigation flow.
    /// </summary>
    /// <remarks>This method is not intended for resource disposal, which should be handled in the Dispose method.</remarks>
    Task OnDestroyAsync();

    /// <summary>
    /// Performs asynchronous cleanup and resource disposal when the object is forcibly destroyed.
    /// </summary>
    /// <remarks>This method is not intended for resource disposal, which should be handled in the Dispose method.</remarks>
    Task OnForceDestroyAsync();

    /// <summary>
    /// Shows the help page associated with this view model.
    /// </summary>
    void ShowHelpPage();

    /// <summary>
    /// Resets the viewmodel to its default state.
    /// </summary>
    void ResetToDefault();
}