using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReactiveUI;
using System;
using velowrench.Core.ViewModels;
using velowrench.Core.ViewModels.Home;
using velowrench.Core.ViewModels.Tools;
using velowrench.UI.Views;
using velowrench.UI.Views.Home;
using velowrench.UI.Views.Tools;

namespace velowrench.UI;

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