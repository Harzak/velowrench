using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace velowrench.UI;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data == null)
        {
            return null;
        }

        string name = data.GetType().FullName!
            .Replace("ViewModel", "View", StringComparison.Ordinal)
            .Replace(".Core.", ".UI.", StringComparison.Ordinal);

        Type? type = Type.GetType(name);

        if (type != null)
        {
            var control = (Control)Activator.CreateInstance(type)!;
            control.DataContext = data; // This was missing!
            return control;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data) => data is not null && data is ObservableObject;
}