using Android.App;
using Android.OS;
using Avalonia.Android;
using Avalonia.Controls;
using velowrench.UI;
using System;

namespace velowrench.Shell.Android.Activities;

public abstract class BaseAvaloniaActivity<TViewModel> : Activity where TViewModel : class
{
    protected TViewModel? ViewModel { get; private set; }
    private AvaloniaView? _avaloniaView;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Create ViewModel
        ViewModel = CreateViewModel();
        if (ViewModel == null) return;

        // Create Avalonia view using ViewLocator
        var viewLocator = new ViewLocator();
        var control = viewLocator.Build(ViewModel);
        
        if (control != null)
        {
            // Ensure DataContext is set (ViewLocator should have done this, but let's be explicit)
            control.DataContext = ViewModel;
            
            _avaloniaView = new AvaloniaView(this);
            _avaloniaView.Content = control;
            SetContentView(_avaloniaView);
        }
        else
        {
            // Fallback if ViewLocator fails
            var errorControl = new TextBlock 
            { 
                Text = $"Failed to create view for {ViewModel.GetType().Name}",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
            };
            
            _avaloniaView = new AvaloniaView(this);
            _avaloniaView.Content = errorControl;
            SetContentView(_avaloniaView);
        }
    }

    protected abstract TViewModel CreateViewModel();

    protected override void OnDestroy()
    {
        _avaloniaView?.Dispose();
        _avaloniaView = null;
        base.OnDestroy();
    }
}   