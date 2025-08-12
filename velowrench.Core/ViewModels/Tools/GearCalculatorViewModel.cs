using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;
using velowrench.Repository.Extensions;
using velowrench.Repository.Interfaces;
using velowrench.Repository.Models;

namespace velowrench.Core.ViewModels.Tools;

public sealed partial class GearCalculatorViewModel : BaseRoutableViewModel
{
    private readonly IComponentStandardRepository _repository;

    public override string Name { get; }

    [ObservableProperty]
    private ObservableCollection<WheelSpecificationModel> _sourceWheels;

    [ObservableProperty]
    private WheelSpecificationModel _selectedWheel;

    [ObservableProperty]
    private ObservableCollection<CranksetSpecificationModel> _sourceCranks;

    [ObservableProperty]
    private CranksetSpecificationModel _selectedCrank;

    [ObservableProperty]
    private ObservableCollection<CadenceModel> _sourceCadence;

    [ObservableProperty]
    private CadenceModel _selectedCadence;

    [ObservableProperty]
    private ObservableCollection<SprocketSpecificationModel> _sourceSprockets;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedSprocketsStr))]
    private List<SprocketSpecificationModel> _selectedSprockets;

    public string SelectedSprocketsStr => string.Join(", ", _selectedSprockets.Select(x => x.Label));

    public GearCalculatorViewModel(INavigationService navigationService, IComponentStandardRepository repository, ILocalizer localizer) : base(navigationService)
    {
        ArgumentNullException.ThrowIfNull(navigationService, nameof(navigationService));
        ArgumentNullException.ThrowIfNull(localizer, nameof(localizer));
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));

        _repository = repository;

        this.Name = localizer.GetString("GearCalculator");
        this.SourceWheels = new(_repository.GetMostCommonWheelSpecifications());
        this.SelectedWheel = this.SourceWheels.GetMostUsedWheel();
        this.SourceCranks = new(_repository.GetAllCranksetSpecifications());
        this.SelectedCrank = this.SourceCranks.GetMostUsedCrankset();
        this.SourceCadence = new(_repository.GetAllCandences());
        this.SelectedCadence = this.SourceCadence.GetMostUsedCadence();
        this.SourceSprockets = new(_repository.GetMostCommonSprocketSpecifications());
        this.SelectedSprockets = [];
    }

    [RelayCommand]
    private void SprocketSelected(SprocketSpecificationModel sprocket)
    {
        if (sprocket == null)
        {
            return;
        }
        if (!this.SelectedSprockets.Remove(sprocket))
        {
            this.SelectedSprockets.Add(sprocket);
        }
        OnPropertyChanged(nameof(SelectedSprocketsStr));
    }
}
