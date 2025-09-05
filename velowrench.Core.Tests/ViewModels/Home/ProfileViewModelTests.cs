using FakeItEasy;
using FluentAssertions;
using UnitsNet.Units;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Home;

namespace velowrench.Core.Tests.ViewModels.Home;

[TestClass]
public class ProfileViewModelTests
{
    private IUnitStore _unitStore;
    private ILocalizer _localizer;
    private INavigationService _navigationService;
    private IToolbar _toolbar;
    private ProfileViewModel _viewModel;

    [TestInitialize]
    public void Initialize()
    {
        _unitStore = A.Fake<IUnitStore>();
        _localizer = A.Fake<ILocalizer>();
        _navigationService = A.Fake<INavigationService>();
        _toolbar = A.Fake<IToolbar>();

        A.CallTo(() => _unitStore.LengthDefaultUnit).Returns(LengthUnit.Millimeter);
        A.CallTo(() => _unitStore.DistanceDefaultUnit).Returns(LengthUnit.Meter);
        A.CallTo(() => _unitStore.SpeedDefaultUnit).Returns(SpeedUnit.KilometerPerHour);
        A.CallTo(() => _localizer.GetString("Settings")).Returns("Settings");

        _viewModel = new(_localizer, _unitStore, _navigationService, _toolbar);
    }

    [TestMethod]
    public async Task OnInitializedAsync_ShouldInitializeWithDefaultValues()
    {
        // Act
        await _viewModel.OnInitializedAsync();

        // Assert
        _viewModel.SelectedLengthUnit.Should().Be(LengthUnit.Millimeter);
        _viewModel.SelectedDistanceUnit.Should().Be(LengthUnit.Meter);
        _viewModel.SelectedSpeedUnit.Should().Be(SpeedUnit.KilometerPerHour);
        _viewModel.SupportedCultures.Should().HaveCount(2);
        _viewModel.AvailableThemes.Should().HaveCount(3);
    }

    [TestMethod]
    public void OnSelectedLengthUnitChanged_ShouldUpdateUnitStore()
    {
        // Arrange
        LengthUnit newUnit = LengthUnit.Inch;

        // Act
        _viewModel.SelectedLengthUnit = newUnit;

        // Assert
        A.CallToSet(() => _unitStore.LengthDefaultUnit).To(newUnit);
    }
}