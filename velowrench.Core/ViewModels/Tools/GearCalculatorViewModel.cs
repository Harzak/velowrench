using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;

namespace velowrench.Core.ViewModels.Tools;

public sealed partial class GearCalculatorViewModel : BaseRoutableViewModel
{
    public override string Name { get; }

    [ObservableProperty]
    private ObservableCollection<Test> _items;

    public GearCalculatorViewModel(INavigationService navigationService, ILocalizer localizer) : base(navigationService)
    {
        ArgumentNullException.ThrowIfNull(navigationService, nameof(navigationService));
        ArgumentNullException.ThrowIfNull(localizer, nameof(localizer));

        Name = localizer.GetString("GearCalculator");
        this.Items = new ObservableCollection<Test>
        {
            new Test()
            {
                A = "ValueA1",
                B = "ValueB1",
                C = "ValueC1" 
            },
           new Test()
            {
                A = "ValueA2",
                B = "ValueB2",
                C = "ValueC2"
            },
             new Test()
            {
                A = "ValueA3",
                B = "ValueB3",
                C = "ValueC3"
            }
        };

    }
}

public class Test
{
    public string A { get; set; } = "ValueA";
    public string B { get; set; } = "ValueB";
    public string C { get; set; } = "ValueC";
}