using FakeItEasy;
using FluentAssertions;
using UnitsNet.Units;
using velowrench.Calculations.Calculators.Wheels.SpokeLength;
using velowrench.Calculations.Interfaces;
using velowrench.Calculations.Validation;
using velowrench.Calculations.Validation.Results;
using velowrench.Core.Interfaces;
using velowrench.Core.Units;
using velowrench.Core.ViewModels.Tools;
using velowrench.Repository.Interfaces;
using velowrench.Repository.Models;
using velowrench.Utils.Enums;
using velowrench.Utils.EventArg;
using velowrench.Utils.Interfaces;
using velowrench.Utils.Results;

namespace velowrench.Core.Tests.ViewModels.Tools;

[TestClass]
public class SpokeLengthCalculatorViewModelTests
{
    private ICalculator<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult> _calculator;
    private ICalculatorInputValidator<SpokeLengthCalculatorInput> _inputValidation;
    private INavigationService _navigationService;
    private ILocalizer _localizer;
    private IComponentStandardRepository _repository;
    private ICalculatorFactory<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult> _calculatorFactory;
    private IDebounceActionFactory _debounceActionFactory;
    private IUnitStore _unitStore;
    private IToolbar _toolbar;
    private IClipboardInterop _clipboardInterop;
    private SpokeLengthCalculatorViewModel _viewModel;

    [TestInitialize]
    public void Initialize()
    {
        _calculatorFactory = A.Fake<ICalculatorFactory<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult>>();
        _debounceActionFactory = A.Fake<IDebounceActionFactory>();
        _navigationService = A.Fake<INavigationService>();
        _localizer = A.Fake<ILocalizer>();
        _repository = A.Fake<IComponentStandardRepository>();
        _calculator = A.Fake<ICalculator<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult>>();
        _inputValidation = A.Fake<ICalculatorInputValidator<SpokeLengthCalculatorInput>>();
        _unitStore = A.Fake<IUnitStore>();
        _toolbar = A.Fake<IToolbar>();
        _clipboardInterop = A.Fake<IClipboardInterop>();
    }

    private void GlobalSetup(ECalculatorState calculatorState, ValidationResult validation)
    {
        A.CallTo(() => _inputValidation.ValidateProperty(A<SpokeLengthCalculatorInput>._, A<string>._, A<ValidationContext>._))
            .Returns(validation);
        A.CallTo(() => _inputValidation.ValidateWithResults(A<SpokeLengthCalculatorInput>._, A<ValidationContext>._))
            .Returns(validation);

        A.CallTo(() => _calculator.InputValidator)
            .Returns(_inputValidation);
        A.CallTo(() => _calculator.State)
            .Returns(calculatorState);

        A.CallTo(() => _calculatorFactory.CreateCalculator())
            .Returns(_calculator);

        A.CallTo(() => _debounceActionFactory.CreateDebounceUIAction(A<Action>._, A<int>._))
            .ReturnsLazily((Action action, int delayMs) => new TestDebounceAction(action));

        // Setup repository mock data
        List<int> spokeCounts =
        [
            24, 28, 32, 36
        ];

        List<SpokeLacingPatternModel> lacingPatterns =
        [
            new("Radial (0-cross)", 0),
            new("1-cross", 1),
            new("2-cross", 2),
            new("3-cross", 3)
        ];

        A.CallTo(() => _repository.GetMostCommonWheelSpokeCount()).Returns(spokeCounts);
        A.CallTo(() => _repository.GetMostCommonSpokeLacingPattern()).Returns(lacingPatterns);

        _viewModel = new(_calculatorFactory, _unitStore, _navigationService, _debounceActionFactory, _repository, _localizer, _toolbar, _clipboardInterop);
    }

    [TestMethod]
    public async Task Calculate_WithValidInputs_ShouldUpdateResults()
    {
        // Arrange
        double expectedSpokeLengthGearSide = 295;
        double expectedSpokeLengthNonGearSide = 290;
        OperationResult<SpokeLengthCalculatorResult> expectedResult = new()
        {
            IsSuccess = true,
            Content = new SpokeLengthCalculatorResult()
            {
                CalculatedAt = DateTime.UtcNow,
                SpokeLengthGearSideMm = expectedSpokeLengthGearSide,
                SpokeLengthNonGearSideMm = expectedSpokeLengthNonGearSide,
                UsedInputs = new SpokeLengthCalculatorInput()
                {
                    HubCenterToFlangeDistanceGearSideMm = 36,
                    HubCenterToFlangeDistanceNonGearSideMm = 17,
                    HubFlangeDiameterGearSideMm = 45,
                    HubFlangeDiameterNonGearSideMm = 45,
                    RimInternalDiameterMm = 600,
                    SpokeCount = 32,
                    SpokeLacingPattern = 3
                }
            }
        };

        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>._)).Returns(expectedResult);
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Act
        _viewModel.HubCenterToFlangeDistanceGearSide!.Value = 36;
        _viewModel.HubCenterToFlangeDistanceNonGearSide!.Value = 17;
        _viewModel.HubFlangeDiameterGearSide!.Value = 45;
        _viewModel.HubFlangeDiameterNonGearSide!.Value = 45;
        _viewModel.RimInternalDiameter!.Value = 600;
        _viewModel.SelectedSpokeCount = 32;
        _viewModel.SelectedSpokeLacingPattern = new SpokeLacingPatternModel("3-cross", 3);

        // Assert
        _viewModel.RecommendedSpokeLengthGearSide.Should().NotBeNull();
        _viewModel.RecommendedSpokeLengthGearSide.Value.Should().Be(expectedSpokeLengthGearSide);
        _viewModel.RecommendedSpokeLengthNonGearSide.Should().NotBeNull();
        _viewModel.RecommendedSpokeLengthNonGearSide.Value.Should().Be(expectedSpokeLengthNonGearSide);
    }

    [TestMethod]
    public async Task OnInputsChanged_WithInvalidInputs_ShouldNotStartCalculation()
    {
        // Arrange
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithError("", ""));
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Act
        _viewModel.HubCenterToFlangeDistanceGearSide!.Value = 0;
        _viewModel.HubCenterToFlangeDistanceNonGearSide!.Value = 17;
        _viewModel.HubFlangeDiameterGearSide!.Value = 45;
        _viewModel.HubFlangeDiameterNonGearSide!.Value = 45;
        _viewModel.RimInternalDiameter!.Value = 600;

        // Assert
        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>._)).MustNotHaveHappened();
        _viewModel.CurrentState.Should().Be(ECalculatorState.NotStarted);
    }

    [TestMethod]
    public async Task OnInputsChanged_WhenCalculationInProgress_ShouldNotStartNewCalculation()
    {
        // Arrange
        this.GlobalSetup(ECalculatorState.InProgress, ValidationResult.WithSuccess());
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Act
        _viewModel.HubCenterToFlangeDistanceGearSide!.Value = 36;
        _viewModel.HubCenterToFlangeDistanceNonGearSide!.Value = 17;
        _viewModel.HubFlangeDiameterGearSide!.Value = 45;
        _viewModel.HubFlangeDiameterNonGearSide!.Value = 45;
        _viewModel.RimInternalDiameter!.Value = 600;

        // Assert
        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public async Task Calculate_WithSameInputsAsPrevious_ShouldNotRecalculate()
    {
        // Arrange
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);
        Fake.ClearRecordedCalls(_calculator);

        // Act - First calculation
        _viewModel.HubCenterToFlangeDistanceGearSide!.Value = 36;
        _viewModel.HubCenterToFlangeDistanceNonGearSide!.Value = 17;
        _viewModel.HubFlangeDiameterGearSide!.Value = 45;
        _viewModel.HubFlangeDiameterNonGearSide!.Value = 45;
        _viewModel.RimInternalDiameter!.Value = 600;
        _viewModel.SelectedSpokeCount = 30;

        // Act - Second time with same inputs
        _viewModel.HubCenterToFlangeDistanceGearSide!.Value = 36;
        _viewModel.HubCenterToFlangeDistanceNonGearSide!.Value = 17;
        _viewModel.HubFlangeDiameterGearSide!.Value = 45;
        _viewModel.HubFlangeDiameterNonGearSide!.Value = 45;
        _viewModel.RimInternalDiameter!.Value = 600;
        _viewModel.SelectedSpokeCount = 30;

        // Assert
        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>._)).MustHaveHappened(6, Times.Exactly);
    }

    [TestMethod]
    public async Task Calculate_WithFailedResult_ShouldNotUpdateResults()
    {
        // Arrange
        OperationResult<SpokeLengthCalculatorResult> failedResult = new()
        {
            IsSuccess = false
        };

        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>._)).Returns(failedResult);
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Act
        _viewModel.HubCenterToFlangeDistanceGearSide!.Value = 36;
        _viewModel.HubCenterToFlangeDistanceNonGearSide!.Value = 17;
        _viewModel.HubFlangeDiameterGearSide!.Value = 45;
        _viewModel.HubFlangeDiameterNonGearSide!.Value = 45;
        _viewModel.RimInternalDiameter!.Value = 600;

        // Assert
        _viewModel.RecommendedSpokeLengthGearSide.Should().BeNull();
        _viewModel.RecommendedSpokeLengthNonGearSide.Should().BeNull();
    }

    [TestMethod]
    public async Task OnSpokeLengthCalculatorStateChanged_DirectStateChange_ShouldUpdateImmediately()
    {
        // Arrange
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);
        CalculatorStateEventArgs eventArgs = new(ECalculatorState.Failed);

        // Act
        _calculator.StateChanged += Raise.With(_calculator, eventArgs);

        // Assert
        _viewModel.CurrentState.Should().Be(ECalculatorState.Failed);
    }

    [TestMethod]
    public async Task HubMeasurementsSelectedUnit_Changed_ShouldUpdateAllHubMeasurements()
    {
        // Arrange
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Act
        _viewModel.HubMeasurementsSelectedUnit = LengthUnit.Inch;

        // Assert
        _viewModel.HubCenterToFlangeDistanceGearSide!.Unit.Should().Be(LengthUnit.Inch);
        _viewModel.HubCenterToFlangeDistanceNonGearSide!.Unit.Should().Be(LengthUnit.Inch);
        _viewModel.HubFlangeDiameterGearSide!.Unit.Should().Be(LengthUnit.Inch);
        _viewModel.HubFlangeDiameterNonGearSide!.Unit.Should().Be(LengthUnit.Inch);
    }

    [TestMethod]
    public async Task RimInternalDiameterSelectedUnit_Changed_ShouldUpdateRimDiameter()
    {
        // Arrange
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Act
        _viewModel.RimInternalDiameterSelectedUnit = LengthUnit.Inch;

        // Assert
        _viewModel.RimInternalDiameter!.Unit.Should().Be(LengthUnit.Inch);
    }

    [TestMethod]
    public async Task SelectedSpokeCount_Changed_ShouldTriggerCalculation()
    {
        // Arrange
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Setup minimal valid inputs first
        _viewModel.HubCenterToFlangeDistanceGearSide!.Value = 36;
        _viewModel.HubCenterToFlangeDistanceNonGearSide!.Value = 17;
        _viewModel.HubFlangeDiameterGearSide!.Value = 45;
        _viewModel.HubFlangeDiameterNonGearSide!.Value = 45;
        _viewModel.RimInternalDiameter!.Value = 600;

        // Act
        _viewModel.SelectedSpokeCount = 36;

        // Assert
        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>.That.Matches(
            input => input.SpokeCount == 36))).MustHaveHappened();
    }

    [TestMethod]
    public async Task SelectedSpokeLacingPattern_Changed_ShouldTriggerCalculation()
    {
        // Arrange
        SpokeLacingPatternModel newLacingPattern = new("2-cross", 2);
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Setup minimal valid inputs first
        _viewModel.HubCenterToFlangeDistanceGearSide!.Value = 36;
        _viewModel.HubCenterToFlangeDistanceNonGearSide!.Value = 17;
        _viewModel.HubFlangeDiameterGearSide!.Value = 45;
        _viewModel.HubFlangeDiameterNonGearSide!.Value = 45;
        _viewModel.RimInternalDiameter!.Value = 600;

        // Act
        _viewModel.SelectedSpokeLacingPattern = newLacingPattern;

        // Assert
        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>.That.Matches(
            input => input.SpokeLacingPattern == 2))).MustHaveHappened();
    }

    [TestMethod]
    public async Task HubCenterToFlangeDistanceGearSideValueChanged_ShouldTriggerCalculation()
    {
        // Arrange
        ConvertibleDouble<LengthUnit> hubCenterToFlangeDistanceGearSide = new(36, LengthUnit.Millimeter);
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Setup minimal valid inputs first
        _viewModel.HubCenterToFlangeDistanceGearSide = hubCenterToFlangeDistanceGearSide;
        _viewModel.HubCenterToFlangeDistanceNonGearSide!.Value = 17;
        _viewModel.HubFlangeDiameterGearSide!.Value = 45;
        _viewModel.HubFlangeDiameterNonGearSide!.Value = 45;
        _viewModel.RimInternalDiameter!.Value = 600;

        // Act
        hubCenterToFlangeDistanceGearSide.Value = 37;

        // Assert
        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>._)).MustHaveHappened();
    }

    [TestMethod]
    public async Task Calculate_WithUnitConversion_ShouldConvertCorrectly()
    {
        // Arrange
        OperationResult<SpokeLengthCalculatorResult> expectedResult = new()
        {
            IsSuccess = true,
            Content = new SpokeLengthCalculatorResult()
            {
                CalculatedAt = DateTime.UtcNow,
                SpokeLengthGearSideMm = 295,
                SpokeLengthNonGearSideMm = 290,
                UsedInputs = new SpokeLengthCalculatorInput()
                {
                    HubCenterToFlangeDistanceGearSideMm =   36,
                    HubCenterToFlangeDistanceNonGearSideMm = 17,
                    HubFlangeDiameterGearSideMm = 45,
                    HubFlangeDiameterNonGearSideMm = 45,
                    RimInternalDiameterMm = 600,
                    SpokeCount = 32,
                    SpokeLacingPattern = 3
                }
            }
        };

        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>.That.Matches(input =>
            Math.Abs(input.HubCenterToFlangeDistanceGearSideMm - 36) < 0.1))).Returns(expectedResult);
        A.CallTo(() => _unitStore.LengthDefaultUnit).Returns(LengthUnit.Millimeter);
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Act - Set inputs in inches, should be converted to millimeters for calculation
        _viewModel.HubMeasurementsSelectedUnit = LengthUnit.Inch;
        _viewModel.HubCenterToFlangeDistanceGearSide!.Value = 1.42;
        _viewModel.HubCenterToFlangeDistanceNonGearSide!.Value = 0.67;
        _viewModel.HubFlangeDiameterGearSide!.Value = 1.77;
        _viewModel.HubFlangeDiameterNonGearSide!.Value = 1.77;
        _viewModel.RimInternalDiameter!.Value = 23.62;
        _viewModel.SelectedSpokeCount = 32;
        _viewModel.SelectedSpokeLacingPattern = new SpokeLacingPatternModel("3-cross", 3);

        // Assert
        _viewModel.RecommendedSpokeLengthGearSide.Should().NotBeNull();
        _viewModel.RecommendedSpokeLengthGearSide.Value.Should().Be(295);
        _viewModel.RecommendedSpokeLengthGearSide.Unit.Should().Be(LengthUnit.Millimeter);
        _viewModel.RecommendedSpokeLengthNonGearSide.Should().NotBeNull();
        _viewModel.RecommendedSpokeLengthNonGearSide.Value.Should().Be(290);
        _viewModel.RecommendedSpokeLengthNonGearSide.Unit.Should().Be(LengthUnit.Millimeter);
    }

    [TestMethod]
    public async Task ValidationBehavior_InitialState_ShouldHaveNoErrors()
    {
        // Arrange & Act
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Assert
        _viewModel.InputErrorMessages.Should().BeEmpty();
        _viewModel.CurrentState.Should().Be(ECalculatorState.NotStarted);
    }

    [TestMethod]
    public async Task ValidationBehavior_UserEntersInvalidValue_ShouldShowError()
    {
        // Arrange
        string errorMessage = "Hub flange diameter for gear side must be between 20 mm and 80 mm.";
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithError("HubFlangeDiameterGearSideMm", errorMessage));
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Act
        _viewModel.HubFlangeDiameterGearSide!.Value = 10; // Below minimum

        // Assert
        _viewModel.InputErrorMessages.Should().NotBeEmpty();
        _viewModel.InputErrorMessages.Should().Contain(msg => msg.Contains(errorMessage));
    }

    [TestMethod]
    public async Task ValidationBehavior_MultipleInvalidInputs_ShouldShowCumulativeErrors()
    {
        // Arrange
        string errorMessage1 = "Hub flange diameter for gear side must be between 20 mm and 80 mm.";
        string errorMessage2 = "Rim internal diameter (ERD) must be between 200 mm and 800 mm.";
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithErrors(
        [
            new ValidationError()
            {
                PropertyName = "HubFlangeDiameterGearSideMm",
                Message = errorMessage1
            },
            new ValidationError()
            {
                PropertyName = "RimInternalDiameterMm",
                Message = errorMessage2
            },
        ]));
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Act 
        _viewModel.HubFlangeDiameterGearSide!.Value = 10; // Below minimum
        _viewModel.RimInternalDiameter!.Value = 100; // Below minimum

        // Assert
        _viewModel.InputErrorMessages.Should().HaveCount(2);
        _viewModel.InputErrorMessages.Should().Contain(msg => msg.Contains(errorMessage1));
        _viewModel.InputErrorMessages.Should().Contain(msg => msg.Contains(errorMessage2));
    }
}