using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using velowrench.Core.ViewModels;

namespace velowrench.UI.Views;

public partial class MainWindow : ReactiveUserControl<MainWindowViewModel>
{
    public MainWindow()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }
}