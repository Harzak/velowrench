using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Core.Navigation.Context;

/// <summary>
/// Navigation context that includes parameters and metadata
/// </summary>
public sealed class NavigationContext
{
    public NavigationParameters Parameters { get; }
    public string? SourceView { get; }
    public DateTime NavigatedAt { get; }

    public NavigationContext(NavigationParameters? parameters = null, string? sourceView = null)
    {
        Parameters = parameters ?? [];
        SourceView = sourceView;
        NavigatedAt = DateTime.UtcNow;
    }
}

