using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReactiveUI;
using System;
using velowrench.ViewModels;
using velowrench.ViewModels.Home;
using velowrench.ViewModels.Tools;
using velowrench.Views;
using velowrench.Views.Home;
using velowrench.Views.Tools;

namespace velowrench.Routing;

public class ViewLocator : IViewLocator
{
    public IViewFor ResolveView<T>(T? viewModel, string? contract = null)
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        return viewModel switch
        {
            MainWindowViewModel => new MainWindow { DataContext = viewModel },
            HomeViewModel => new HomeView { DataContext = viewModel },
            ChainLengthCalculatorViewModel => new ChainLengthCalculatorView { DataContext = viewModel },
            ChainlineCalculatorViewModel => new ChainlineCalculatorView { DataContext = viewModel },
            DrivetrainRatioCalculatorViewModel => new DrivetrainRatioCalculatorView { DataContext = viewModel },
            RolloutCalculatorViewModel => new RolloutCalculatorView { DataContext = viewModel },
            _ => throw new NotImplementedException(nameof(viewModel)),
        };
    }
}