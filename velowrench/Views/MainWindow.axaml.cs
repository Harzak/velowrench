using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using velowrench.ViewModels;

namespace velowrench.Views;

public partial class MainWindow : ReactiveUserControl<MainWindowViewModel>
{
    public MainWindow()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }
}