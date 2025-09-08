using Avalonia;
using Avalonia.Styling;
using FakeItEasy;
using FluentAssertions;
using System.Globalization;
using UnitsNet.Units;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Configuration;
using velowrench.Repository.Interfaces;
using velowrench.Repository.Models;
using velowrench.Utils.Results;

namespace velowrench.Core.Tests.Configuration;

[TestClass]
public class AppConfigurationTests
{
    private IUserPreferenceRepository _userPreferenceRepository;
    private IUnitStore _unitStore;
    private AppConfiguration _appConfiguration;

    [TestInitialize]
    public void Initialize()
    {
        _userPreferenceRepository = A.Fake<IUserPreferenceRepository>();
        _unitStore = A.Fake<IUnitStore>();

        A.CallTo(() => _unitStore.LengthDefaultUnit).Returns(LengthUnit.Millimeter);
        A.CallTo(() => _unitStore.DistanceDefaultUnit).Returns(LengthUnit.Meter);
        A.CallTo(() => _unitStore.SpeedDefaultUnit).Returns(SpeedUnit.KilometerPerHour);
        
        _appConfiguration = new AppConfiguration(_userPreferenceRepository, _unitStore);
    }


    [TestMethod]
    public void Constructor_WithValidParameters_ShouldInitializeCollectionsCorrectly()
    {
        // Arrange & Act
        AppConfiguration configuration = new(_userPreferenceRepository, _unitStore);

        // Assert
        configuration.SupportedCultures.Should().NotBeNull();
        configuration.SupportedCultures.Should().HaveCount(2);
        configuration.SupportedCultures.Should().Contain(c => c.Name == "en-US");
        configuration.SupportedCultures.Should().Contain(c => c.Name == "fr-FR");

        configuration.AvailableThemes.Should().NotBeNull();
        configuration.AvailableThemes.Should().HaveCount(3);
        configuration.AvailableThemes.Should().Contain(ThemeVariant.Light);
        configuration.AvailableThemes.Should().Contain(ThemeVariant.Dark);
        configuration.AvailableThemes.Should().Contain(ThemeVariant.Default);

        configuration.AppVersion.Should().NotBeNull();
    }

    [TestMethod]
    public void CurrentCulture_SetWithSupportedCulture_ShouldUpdateCurrentCulture()
    {
        // Arrange
        CultureInfo frenchCulture = new("fr-FR");

        // Act
        _appConfiguration.CurrentCulture = frenchCulture;

        // Assert
        _appConfiguration.CurrentCulture.Should().Be(frenchCulture);
        CultureInfo.DefaultThreadCurrentCulture.Should().Be(frenchCulture);
    }

    [TestMethod]
    public void CurrentCulture_SetWithUnsupportedCulture_ShouldThrowInvalidOperationException()
    {
        // Arrange
        CultureInfo unsupportedCulture = new("de-DE");

        // Act & Assert
        Action action = () => _appConfiguration.CurrentCulture = unsupportedCulture;
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Unsupported culture");
    }

    [TestMethod]
    public void CurrentTheme_SetWithSupportedTheme_ShouldUpdateCurrentTheme()
    {
        // Arrange & Act
        _appConfiguration.CurrentTheme = ThemeVariant.Dark;

        // Assert
        _appConfiguration.CurrentTheme.Should().Be(ThemeVariant.Dark);
    }

    [TestMethod]
    public void CurrentTheme_SetWithUnsupportedTheme_ShouldThrowInvalidOperationException()
    {
        // Arrange
        ThemeVariant unsupportedTheme = new("CustomTheme", null);

        // Act & Assert
        Action action = () => _appConfiguration.CurrentTheme = unsupportedTheme;
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Unsupported theme");
    }

    [TestMethod]
    public void PreferredLengthUnit_SetWithValidUnit_ShouldUpdateBothConfigurationAndUnitStore()
    {
        // Arrange
        LengthUnit newUnit = LengthUnit.Inch;

        // Act
        _appConfiguration.PreferredLengthUnit = newUnit;

        // Assert
        _appConfiguration.PreferredLengthUnit.Should().Be(newUnit);
        A.CallToSet(() => _unitStore.LengthDefaultUnit).To(newUnit).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void PreferredLengthUnit_SetWithSameUnit_ShouldNotUpdateUnitStore()
    {
        // Arrange
        LengthUnit sameUnit = LengthUnit.Millimeter;
        _appConfiguration.PreferredLengthUnit = sameUnit;
        Fake.ClearRecordedCalls(_unitStore);

        // Act
        _appConfiguration.PreferredLengthUnit = sameUnit;

        // Assert
        A.CallToSet(() => _unitStore.LengthDefaultUnit).MustNotHaveHappened();
    }

    [TestMethod]
    public void PreferredDistanceUnit_SetWithValidUnit_ShouldUpdateBothConfigurationAndUnitStore()
    {
        // Arrange
        LengthUnit newUnit = LengthUnit.Kilometer;

        // Act
        _appConfiguration.PreferredDistanceUnit = newUnit;

        // Assert
        _appConfiguration.PreferredDistanceUnit.Should().Be(newUnit);
        A.CallToSet(() => _unitStore.DistanceDefaultUnit).To(newUnit).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void PreferredDistanceUnit_SetWithSameUnit_ShouldNotUpdateUnitStore()
    {
        // Arrange
        LengthUnit sameUnit = LengthUnit.Meter;
        _appConfiguration.PreferredDistanceUnit = sameUnit;
        Fake.ClearRecordedCalls(_unitStore);

        // Act
        _appConfiguration.PreferredDistanceUnit = sameUnit;

        // Assert
        A.CallToSet(() => _unitStore.DistanceDefaultUnit).MustNotHaveHappened();
    }

    [TestMethod]
    public void PreferredSpeedUnit_SetWithValidUnit_ShouldUpdateBothConfigurationAndUnitStore()
    {
        // Arrange
        SpeedUnit newUnit = SpeedUnit.MilePerHour;

        // Act
        _appConfiguration.PreferredSpeedUnit = newUnit;

        // Assert
        _appConfiguration.PreferredSpeedUnit.Should().Be(newUnit);
        A.CallToSet(() => _unitStore.SpeedDefaultUnit).To(newUnit).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void PreferredSpeedUnit_SetWithSameUnit_ShouldNotUpdateUnitStore()
    {
        // Arrange
        SpeedUnit sameUnit = SpeedUnit.KilometerPerHour;
        _appConfiguration.PreferredSpeedUnit = sameUnit;
        Fake.ClearRecordedCalls(_unitStore);

        // Act
        _appConfiguration.PreferredSpeedUnit = sameUnit;

        // Assert
        A.CallToSet(() => _unitStore.SpeedDefaultUnit).MustNotHaveHappened();
    }

    [TestMethod]
    public async Task InitializeAsync_WhenLoadPreferencesSucceeds_ShouldLoadSavedPreferences()
    {
        // Arrange
        UserPreferenceModel savedPreferences = new()
        {
            AppLanguage = "fr-FR",
            Theme = "Dark",
            LengthUnit = "Inch",
            DistanceUnit = "Kilometer", 
            SpeedUnit = "MilePerHour"
        };
        OperationResult<UserPreferenceModel> successResult = new(savedPreferences, true);

        A.CallTo(() => _userPreferenceRepository.LoadAsync()).Returns(successResult);

        // Act
        await _appConfiguration.InitializeAsync().ConfigureAwait(false);

        // Assert
        _appConfiguration.CurrentCulture.Name.Should().Be("fr-FR");
        _appConfiguration.CurrentTheme.Should().Be(ThemeVariant.Dark);
        _appConfiguration.PreferredLengthUnit.Should().Be(LengthUnit.Inch);
        _appConfiguration.PreferredDistanceUnit.Should().Be(LengthUnit.Kilometer);
        _appConfiguration.PreferredSpeedUnit.Should().Be(SpeedUnit.MilePerHour);
    }

    [TestMethod]
    public async Task InitializeAsync_WhenLoadPreferencesFails_ShouldLoadDefaultPreferences()
    {
        // Arrange
        OperationResult<UserPreferenceModel> failureResult = new();

        A.CallTo(() => _userPreferenceRepository.LoadAsync()).Returns(failureResult);

        // Act
        await _appConfiguration.InitializeAsync().ConfigureAwait(false);

        // Assert
        _appConfiguration.CurrentCulture.Name.Should().Be("en-US");
        _appConfiguration.PreferredLengthUnit.Should().Be(LengthUnit.Millimeter);
        _appConfiguration.PreferredDistanceUnit.Should().Be(LengthUnit.Meter);
        _appConfiguration.PreferredSpeedUnit.Should().Be(SpeedUnit.KilometerPerHour);
    }

    [TestMethod]
    public async Task InitializeAsync_WhenLoadPreferencesSucceedsButNoContent_ShouldLoadDefaultPreferences()
    {
        // Arrange
        OperationResult<UserPreferenceModel> successButEmptyResult = new OperationResult<UserPreferenceModel>().WithSuccess();

        A.CallTo(() => _userPreferenceRepository.LoadAsync()) .Returns(successButEmptyResult);

        // Act
        await _appConfiguration.InitializeAsync().ConfigureAwait(false);

        // Assert
        _appConfiguration.CurrentCulture.Name.Should().Be("en-US");
        _appConfiguration.PreferredLengthUnit.Should().Be(LengthUnit.Millimeter);
        _appConfiguration.PreferredDistanceUnit.Should().Be(LengthUnit.Meter);
        _appConfiguration.PreferredSpeedUnit.Should().Be(SpeedUnit.KilometerPerHour);
    }

    [TestMethod]
    public async Task InitializeAsync_WithUnsupportedCultureInPreferences_ShouldFallBackToDefault()
    {
        // Arrange
        UserPreferenceModel savedPreferences = new()
        {
            AppLanguage = "de-DE", // Unsupported culture
            Theme = "Dark",
            LengthUnit = "Inch",
            DistanceUnit = "Kilometer",
            SpeedUnit = "MilePerHour"
        };
        OperationResult<UserPreferenceModel> successResult = new(savedPreferences, true);

        A.CallTo(() => _userPreferenceRepository.LoadAsync())
            .Returns(successResult);

        // Act
        await _appConfiguration.InitializeAsync().ConfigureAwait(false);

        // Assert
        _appConfiguration.CurrentCulture.Name.Should().Be("en-US"); // Fallback to default
        _appConfiguration.CurrentTheme.Should().Be(ThemeVariant.Dark); // Other settings should still work
        _appConfiguration.PreferredLengthUnit.Should().Be(LengthUnit.Inch);
    }

    [TestMethod]
    public async Task InitializeAsync_WithUnsupportedThemeInPreferences_ShouldFallBackToDefault()
    {
        // Arrange
        UserPreferenceModel savedPreferences = new()
        {
            AppLanguage = "fr-FR",
            Theme = "CustomTheme", // Unsupported theme
            LengthUnit = "Inch",
            DistanceUnit = "Kilometer",
            SpeedUnit = "MilePerHour"
        };
        OperationResult<UserPreferenceModel> successResult = new(savedPreferences, true);

        A.CallTo(() => _userPreferenceRepository.LoadAsync())
            .Returns(successResult);

        // Act
        await _appConfiguration.InitializeAsync().ConfigureAwait(false);

        // Assert
        _appConfiguration.CurrentCulture.Name.Should().Be("fr-FR"); // Other settings should still work
        _appConfiguration.CurrentTheme.Should().Be(ThemeVariant.Default); // Fallback to default
        _appConfiguration.PreferredLengthUnit.Should().Be(LengthUnit.Inch);
    }

    [TestMethod]
    public async Task InitializeAsync_WithInvalidUnitsInPreferences_ShouldFallBackToDefaults()
    {
        // Arrange
        UserPreferenceModel savedPreferences = new()
        {
            AppLanguage = "fr-FR",
            Theme = "Dark",
            LengthUnit = "InvalidUnit",
            DistanceUnit = "AnotherInvalidUnit",
            SpeedUnit = "YetAnotherInvalidUnit"
        };
        OperationResult<UserPreferenceModel> successResult = new(savedPreferences, true);

        A.CallTo(() => _userPreferenceRepository.LoadAsync())
            .Returns(successResult);

        // Act
        await _appConfiguration.InitializeAsync().ConfigureAwait(false);

        // Assert
        _appConfiguration.CurrentCulture.Name.Should().Be("fr-FR");
        _appConfiguration.CurrentTheme.Should().Be(ThemeVariant.Dark);
        _appConfiguration.PreferredLengthUnit.Should().Be(LengthUnit.Millimeter); // Fallback to unit store default
        _appConfiguration.PreferredDistanceUnit.Should().Be(LengthUnit.Meter); // Fallback to unit store default
        _appConfiguration.PreferredSpeedUnit.Should().Be(SpeedUnit.KilometerPerHour); // Fallback to unit store default
    }

    [TestMethod]
    public async Task SaveAsync_ShouldCreateCorrectUserPreferenceModel()
    {
        // Arrange
        _appConfiguration.CurrentCulture = new CultureInfo("fr-FR");
        _appConfiguration.CurrentTheme = ThemeVariant.Dark;
        _appConfiguration.PreferredLengthUnit = LengthUnit.Inch;
        _appConfiguration.PreferredDistanceUnit = LengthUnit.Kilometer;
        _appConfiguration.PreferredSpeedUnit = SpeedUnit.MilePerHour;

        // Act
        await _appConfiguration.SaveAsync().ConfigureAwait(false);

        // Assert
        A.CallTo(() => _userPreferenceRepository.SaveAsync(A<UserPreferenceModel>.That.Matches(
            model => model.AppLanguage == "fr-FR" &&
                     model.Theme == "Dark" &&
                     model.LengthUnit == "Inch" &&
                     model.DistanceUnit == "Kilometer" &&
                     model.SpeedUnit == "MilePerHour")))
            .MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public async Task SaveAsync_WithDefaultValues_ShouldSaveCorrectDefaults()
    {
        // Arrange
        await _appConfiguration.InitializeAsync().ConfigureAwait(false); // Initialize with defaults

        // Act
        await _appConfiguration.SaveAsync().ConfigureAwait(false);

        // Assert
        A.CallTo(() => _userPreferenceRepository.SaveAsync(A<UserPreferenceModel>.That.Matches(
            model => model.AppLanguage == "en-US" &&
                     model.Theme == "Default" &&
                     model.LengthUnit == "Millimeter" &&
                     model.DistanceUnit == "Meter" &&
                     model.SpeedUnit == "KilometerPerHour")))
            .MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public async Task LoadAndSaveCycle_ShouldPreserveSettings()
    {
        // Arrange
        UserPreferenceModel originalPreferences = new()
        {
            AppLanguage = "fr-FR",
            Theme = "Light",
            LengthUnit = "Centimeter",
            DistanceUnit = "Foot",
            SpeedUnit = "MilePerHour"
        };
        OperationResult<UserPreferenceModel> loadResult = new(originalPreferences, true);

        A.CallTo(() => _userPreferenceRepository.LoadAsync())
            .Returns(loadResult);

        // Act
        await _appConfiguration.InitializeAsync().ConfigureAwait(false);
        await _appConfiguration.SaveAsync().ConfigureAwait(false);

        // Assert
        A.CallTo(() => _userPreferenceRepository.SaveAsync(A<UserPreferenceModel>.That.Matches(
            model => model.AppLanguage == originalPreferences.AppLanguage &&
                     model.Theme == originalPreferences.Theme &&
                     model.LengthUnit == originalPreferences.LengthUnit &&
                     model.DistanceUnit == originalPreferences.DistanceUnit &&
                     model.SpeedUnit == originalPreferences.SpeedUnit)))
            .MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void AppVersion_Property_ShouldNotBeNull()
    {
        // Arrange & Act
        Version version = _appConfiguration.AppVersion;

        // Assert
        version.Should().NotBeNull();
    }
}