using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReactiveUI;
using System;
using velowrench.ViewModels;
using velowrench.Views;

namespace velowrench;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is null)
            return null;

        var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ReactiveObject;
    }
}

public class AppViewLocator : IViewLocator
{
    public IViewFor ResolveView<T>(T? viewModel, string? contract = null)
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        switch (viewModel)
        {
            case HomeViewModel:
                return new HomeView
                {
                    DataContext = viewModel
                };
            case ChainLengthCalculatorViewModel:
                return new ChainLengthCalculatorView
                {
                    DataContext = viewModel
                };
            default:
                throw new NotImplementedException(nameof(viewModel));
        }
    }
}