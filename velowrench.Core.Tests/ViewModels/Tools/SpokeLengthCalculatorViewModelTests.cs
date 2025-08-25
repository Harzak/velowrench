using FakeItEasy;
using FluentAssertions;
using UnitsNet.Units;
using velowrench.Calculations.Calculators.Wheels.SpokeLength;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Interfaces;
using velowrench.Core.Units;
using velowrench.Core.ViewModels.Tools;
using velowrench.Repository.Interfaces;
using velowrench.Repository.Models;
using velowrench.Utils.Enums;
using velowrench.Utils.EventArg;
using velowrench.Utils.Results;

namespace velowrench.Core.Tests.ViewModels.Tools;

[TestClass]
public class SpokeLengthCalculatorViewModelTests
{
    private ICalculator<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult> _calculator;
    private ICalculatorInputValidation<SpokeLengthCalculatorInput> _inputValidation;
    private INavigationService _navigationService;
    private ILocalizer _localizer;
    private IComponentStandardRepository _repository;
    private SpokeLengthCalculatorViewModel _viewModel;

    [TestInitialize]
    public void Initialize()
    {
        ICalculatorFactory<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult> calculFactory = A.Fake<ICalculatorFactory<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult>>();
        IDebounceActionFactory actionFactory = A.Fake<IDebounceActionFactory>();
        _navigationService = A.Fake<INavigationService>();
        _localizer = A.Fake<ILocalizer>();
        _repository = A.Fake<IComponentStandardRepository>();
        _calculator = A.Fake<ICalculator<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult>>();
        _inputValidation = A.Fake<ICalculatorInputValidation<SpokeLengthCalculatorInput>>();

        A.CallTo(() => calculFactory.CreateCalculator()).Returns(_calculator);
        A.CallTo(() => _calculator.GetValidation()).Returns(_inputValidation);

        A.CallTo(() => actionFactory.CreateDebounceUIAction(A<Action>._, A<int>._))
        .ReturnsLazily((Action action, int delayMs) =>
        {
            return new TestDebounceAction(action);
        });

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

        _viewModel = new(calculFactory, _navigationService, actionFactory, _repository, _localizer);
    }

    [TestMethod]
    public void Calculate_WithValidInputs_ShouldUpdateResults()
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
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.NotStarted);
        A.CallTo(() => _inputValidation.Validate(A<SpokeLengthCalculatorInput>._)).Returns(true);

        // Act
        _viewModel.HubCenterToFlangeDistanceGearSide.Value = 36;
        _viewModel.HubCenterToFlangeDistanceNonGearSide.Value = 17;
        _viewModel.HubFlangeDiameterGearSide.Value = 45;
        _viewModel.HubFlangeDiameterNonGearSide.Value = 45;
        _viewModel.RimInternalDiameter.Value = 600;
        _viewModel.SelectedSpokeCount = 32;
        _viewModel.SelectedSpokeLacingPattern = new SpokeLacingPatternModel("3-cross", 3);

        // Assert
        _viewModel.RecommendedSpokeLengthGearSide.Should().NotBeNull();
        _viewModel.RecommendedSpokeLengthGearSide.Value.Should().Be(expectedSpokeLengthGearSide);
        _viewModel.RecommendedSpokeLengthNonGearSide.Should().NotBeNull();
        _viewModel.RecommendedSpokeLengthNonGearSide.Value.Should().Be(expectedSpokeLengthNonGearSide);
    }

    [TestMethod]
    public void OnInputsChanged_WithInvalidInputs_ShouldNotStartCalculation()
    {
        // Arrange
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.NotStarted);
        A.CallTo(() => _inputValidation.Validate(A<SpokeLengthCalculatorInput>._)).Returns(false);

        // Act
        _viewModel.HubCenterToFlangeDistanceGearSide.Value = 0;
        _viewModel.HubCenterToFlangeDistanceNonGearSide.Value = 17;
        _viewModel.HubFlangeDiameterGearSide.Value = 45;
        _viewModel.HubFlangeDiameterNonGearSide.Value = 45;
        _viewModel.RimInternalDiameter.Value = 600;

        // Assert
        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>._)).MustNotHaveHappened();
        _viewModel.CurrentState.Should().Be(ECalculatorState.NotStarted);
    }

    [TestMethod]
    public void OnInputsChanged_WithPartialInputs_ShouldNotStartCalculation()
    {
        // Arrange
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.NotStarted);

        // Act
        _viewModel.HubCenterToFlangeDistanceGearSide.Value = 36;
        _viewModel.HubCenterToFlangeDistanceNonGearSide.Value = 17;

        // Assert
        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public void OnInputsChanged_WhenCalculationInProgress_ShouldNotStartNewCalculation()
    {
        // Arrange
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.InProgress);

        // Act
        _viewModel.HubCenterToFlangeDistanceGearSide.Value = 36;
        _viewModel.HubCenterToFlangeDistanceNonGearSide.Value = 17;
        _viewModel.HubFlangeDiameterGearSide.Value = 45;
        _viewModel.HubFlangeDiameterNonGearSide.Value = 45;
        _viewModel.RimInternalDiameter.Value = 600;

        // Assert
        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public void Calculate_WithSameInputsAsPrevious_ShouldNotRecalculate()
    {
        // Arrange
        SpokeLengthCalculatorInput input = new()
        {
            HubCenterToFlangeDistanceGearSideMm = 36,
            HubCenterToFlangeDistanceNonGearSideMm = 17,
            HubFlangeDiameterGearSideMm = 45,
            HubFlangeDiameterNonGearSideMm = 45,
            RimInternalDiameterMm = 600,
            SpokeCount = 30,
            SpokeLacingPattern = 2
        };

        OperationResult<SpokeLengthCalculatorResult> result = new()
        {
            IsSuccess = true,
            Content = new SpokeLengthCalculatorResult()
            {
                CalculatedAt = DateTime.UtcNow,
                SpokeLengthGearSideMm = 295,
                SpokeLengthNonGearSideMm = 290,
                UsedInputs = input
            }
        };

        A.CallTo(() => _inputValidation.Validate(A<SpokeLengthCalculatorInput>._)).Returns(true);
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.NotStarted);
        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>._)).Returns(result);

        // Act - First calculation
        _viewModel.HubCenterToFlangeDistanceGearSide.Value = 36;
        _viewModel.HubCenterToFlangeDistanceNonGearSide.Value = 17;
        _viewModel.HubFlangeDiameterGearSide.Value = 45;
        _viewModel.HubFlangeDiameterNonGearSide.Value = 45;
        _viewModel.RimInternalDiameter.Value = 600;
        _viewModel.SelectedSpokeCount = 30;

        // Act - Second time with same inputs
        _viewModel.HubCenterToFlangeDistanceGearSide.Value = 36;
        _viewModel.HubCenterToFlangeDistanceNonGearSide.Value = 17;
        _viewModel.HubFlangeDiameterGearSide.Value = 45;
        _viewModel.HubFlangeDiameterNonGearSide.Value = 45;
        _viewModel.RimInternalDiameter.Value = 600;
        _viewModel.SelectedSpokeCount = 30;

        // Assert
        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>._)).MustHaveHappened(6, Times.Exactly);
    }

    [TestMethod]
    public void Calculate_WithFailedResult_ShouldNotUpdateResults()
    {
        // Arrange
        OperationResult<SpokeLengthCalculatorResult> failedResult = new()
        {
            IsSuccess = false
        };

        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>._)).Returns(failedResult);
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.NotStarted);

        // Act
        _viewModel.HubCenterToFlangeDistanceGearSide.Value = 36;
        _viewModel.HubCenterToFlangeDistanceNonGearSide.Value = 17;
        _viewModel.HubFlangeDiameterGearSide.Value = 45;
        _viewModel.HubFlangeDiameterNonGearSide.Value = 45;
        _viewModel.RimInternalDiameter.Value = 600;

        // Assert
        _viewModel.RecommendedSpokeLengthGearSide.Should().BeNull();
        _viewModel.RecommendedSpokeLengthNonGearSide.Should().BeNull();
    }

    [TestMethod]
    public void OnSpokeLengthCalculatorStateChanged_DirectStateChange_ShouldUpdateImmediately()
    {
        // Arrange
        CalculatorStateEventArgs eventArgs = new(ECalculatorState.Failed);

        // Act
        _calculator.StateChanged += Raise.With(_calculator, eventArgs);

        // Assert
        _viewModel.CurrentState.Should().Be(ECalculatorState.Failed);
    }

    [TestMethod]
    public void HubMeasurementsSelectedUnit_Changed_ShouldUpdateAllHubMeasurements()
    {
        // Arrange
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.NotStarted);

        // Act
        _viewModel.HubMeasurementsSelectedUnit = LengthUnit.Inch;

        // Assert
        _viewModel.HubCenterToFlangeDistanceGearSide.Unit.Should().Be(LengthUnit.Inch);
        _viewModel.HubCenterToFlangeDistanceNonGearSide.Unit.Should().Be(LengthUnit.Inch);
        _viewModel.HubFlangeDiameterGearSide.Unit.Should().Be(LengthUnit.Inch);
        _viewModel.HubFlangeDiameterNonGearSide.Unit.Should().Be(LengthUnit.Inch);
    }

    [TestMethod]
    public void RimInternalDiameterSelectedUnit_Changed_ShouldUpdateRimDiameter()
    {
        // Arrange
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.NotStarted);

        // Act
        _viewModel.RimInternalDiameterSelectedUnit = LengthUnit.Inch;

        // Assert
        _viewModel.RimInternalDiameter.Unit.Should().Be(LengthUnit.Inch);
    }

    [TestMethod]
    public void SelectedSpokeCount_Changed_ShouldTriggerCalculation()
    {
        // Arrange
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.NotStarted);
        A.CallTo(() => _inputValidation.Validate(A<SpokeLengthCalculatorInput>._)).Returns(true);

        OperationResult<SpokeLengthCalculatorResult> result = new()
        {
            IsSuccess = true,
            Content = new SpokeLengthCalculatorResult()
            {
                CalculatedAt = DateTime.UtcNow,
                SpokeLengthGearSideMm = 295,
                SpokeLengthNonGearSideMm = 290,
                UsedInputs = new SpokeLengthCalculatorInput()
                {
                    HubCenterToFlangeDistanceGearSideMm = 36,
                    HubCenterToFlangeDistanceNonGearSideMm =    17,
                    HubFlangeDiameterGearSideMm = 45,
                    HubFlangeDiameterNonGearSideMm = 45,
                    RimInternalDiameterMm = 600,
                    SpokeCount = 36,
                    SpokeLacingPattern = 3
                }
            }
        };

        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>._)).Returns(result);

        // Setup minimal valid inputs first
        _viewModel.HubCenterToFlangeDistanceGearSide.Value = 36;
        _viewModel.HubCenterToFlangeDistanceNonGearSide.Value = 17;
        _viewModel.HubFlangeDiameterGearSide.Value = 45;
        _viewModel.HubFlangeDiameterNonGearSide.Value = 45;
        _viewModel.RimInternalDiameter.Value = 600;

        // Act
        _viewModel.SelectedSpokeCount = 36;

        // Assert
        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>.That.Matches(
            input => input.SpokeCount == 36))).MustHaveHappened();
    }

    [TestMethod]
    public void SelectedSpokeLacingPattern_Changed_ShouldTriggerCalculation()
    {
        // Arrange
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.NotStarted);
        A.CallTo(() => _inputValidation.Validate(A<SpokeLengthCalculatorInput>._)).Returns(true);

        SpokeLacingPatternModel newLacingPattern = new("2-cross", 2);

        OperationResult<SpokeLengthCalculatorResult> result = new()
        {
            IsSuccess = true,
            Content = new SpokeLengthCalculatorResult()
            {
                CalculatedAt = DateTime.UtcNow,
                SpokeLengthGearSideMm = 295,
                SpokeLengthNonGearSideMm = 290,
                UsedInputs = new SpokeLengthCalculatorInput()
                {
                    HubCenterToFlangeDistanceGearSideMm = 36,
                    HubCenterToFlangeDistanceNonGearSideMm = 17,
                    HubFlangeDiameterGearSideMm = 45,
                    HubFlangeDiameterNonGearSideMm = 45,
                    RimInternalDiameterMm = 600,
                    SpokeCount = 32,
                    SpokeLacingPattern = 2
                }
            }
        };

        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>._)).Returns(result);

        // Setup minimal valid inputs first
        _viewModel.HubCenterToFlangeDistanceGearSide.Value = 36;
        _viewModel.HubCenterToFlangeDistanceNonGearSide.Value = 17;
        _viewModel.HubFlangeDiameterGearSide.Value = 45;
        _viewModel.HubFlangeDiameterNonGearSide.Value = 45;
        _viewModel.RimInternalDiameter.Value = 600;

        // Act
        _viewModel.SelectedSpokeLacingPattern = newLacingPattern;

        // Assert
        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>.That.Matches(
            input => input.SpokeLacingPattern == 2))).MustHaveHappened();
    }

    [TestMethod]
    public void HubCenterToFlangeDistanceGearSideValueChanged_ShouldTriggerCalculation()
    {
        // Arrange
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.NotStarted);
        A.CallTo(() => _inputValidation.Validate(A<SpokeLengthCalculatorInput>._)).Returns(true);

        ConvertibleDouble<LengthUnit> hubCenterToFlangeDistanceGearSide = new(36, LengthUnit.Millimeter);

        OperationResult<SpokeLengthCalculatorResult> result = new()
        {
            IsSuccess = true,
            Content = new SpokeLengthCalculatorResult()
            {
                CalculatedAt = DateTime.UtcNow,
                SpokeLengthGearSideMm = 295,
                SpokeLengthNonGearSideMm = 290,
                UsedInputs = new SpokeLengthCalculatorInput()
                {
                    HubCenterToFlangeDistanceGearSideMm =   37,
                    HubCenterToFlangeDistanceNonGearSideMm = 17,
                    HubFlangeDiameterGearSideMm = 45,
                    HubFlangeDiameterNonGearSideMm = 45,
                    RimInternalDiameterMm = 600,
                    SpokeCount = 32,
                    SpokeLacingPattern = 3
                }
            }
        };

        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>._)).Returns(result);

        // Setup minimal valid inputs first
        _viewModel.HubCenterToFlangeDistanceGearSide = hubCenterToFlangeDistanceGearSide;
        _viewModel.HubCenterToFlangeDistanceNonGearSide.Value =17;
        _viewModel.HubFlangeDiameterGearSide.Value = 45;
        _viewModel.HubFlangeDiameterNonGearSide.Value = 45;
        _viewModel.RimInternalDiameter.Value = 600;

        // Act
        hubCenterToFlangeDistanceGearSide.Value = 37;

        // Assert
        A.CallTo(() => _calculator.Start(A<SpokeLengthCalculatorInput>._)).MustHaveHappened();
    }

    [TestMethod]
    public void Calculate_WithUnitConversion_ShouldConvertCorrectly()
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
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.NotStarted);
        A.CallTo(() => _inputValidation.Validate(A<SpokeLengthCalculatorInput>._)).Returns(true);

        // Act - Set inputs in inches, should be converted to millimeters for calculation
        _viewModel.HubMeasurementsSelectedUnit = LengthUnit.Inch;
        _viewModel.HubCenterToFlangeDistanceGearSide.Value = 1.42;
        _viewModel.HubCenterToFlangeDistanceNonGearSide.Value = 0.67;
        _viewModel.HubFlangeDiameterGearSide.Value = 1.77;
        _viewModel.HubFlangeDiameterNonGearSide.Value = 1.77;
        _viewModel.RimInternalDiameter.Value = 23.62;
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
    public void ValidationBehavior_InitialState_ShouldHaveNoErrors()
    {
        // Arrange & Act
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.NotStarted);

        // Assert
        _viewModel.InputErrorMessages.Should().BeEmpty();
        _viewModel.CurrentState.Should().Be(ECalculatorState.NotStarted);
    }

    [TestMethod]
    public void ValidationBehavior_UserEntersInvalidValue_ShouldShowError()
    {
        // Arrange
        string errorMessage = "Hub flange diameter for gear side must be between 20 mm and 80 mm.";
        A.CallTo(() => _inputValidation.Validate(A<SpokeLengthCalculatorInput>._)).Returns(false);
        A.CallTo(() => _inputValidation.ErrorMessages).Returns(new Dictionary<string, string>
    {
        { "HubFlangeDiameterGearSideMm", errorMessage }
    });

        // Act
        _viewModel.HubFlangeDiameterGearSide.Value = 10; // Below minimum

        // Assert
        _viewModel.InputErrorMessages.Should().NotBeEmpty();
        _viewModel.InputErrorMessages.Should().Contain(msg => msg.Contains(errorMessage));
    }

    [TestMethod]
    public void ValidationBehavior_MultipleInvalidInputs_ShouldShowCumulativeErrors()
    {
        // Arrange
        string errorMessage1 = "Hub flange diameter for gear side must be between 20 mm and 80 mm.";
        string errorMessage2 = "Rim internal diameter (ERD) must be between 200 mm and 800 mm.";
        A.CallTo(() => _inputValidation.Validate(A<SpokeLengthCalculatorInput>._)).Returns(false);
        A.CallTo(() => _inputValidation.ErrorMessages).Returns(new Dictionary<string, string>
    {
        { "HubFlangeDiameterGearSideMm", errorMessage1 },
        { "RimInternalDiameterMm", errorMessage2 }
    });

        // Act 
        _viewModel.HubFlangeDiameterGearSide.Value = 10; // Below minimum
        _viewModel.RimInternalDiameter.Value = 100; // Below minimum

        // Assert
        _viewModel.InputErrorMessages.Should().HaveCount(2);
        _viewModel.InputErrorMessages.Should().Contain(msg => msg.Contains(errorMessage1));
        _viewModel.InputErrorMessages.Should().Contain(msg => msg.Contains(errorMessage2));
    }

    [TestMethod]
    public void ValidationBehavior_UserCorrectsInvalidValue_ShouldClearSpecificError()
    {
        // Arrange
        string errorMessage1 = "Hub flange diameter for gear side must be between 20 mm and 80 mm.";
        string errorMessage2 = "Rim internal diameter (ERD) must be between 200 mm and 800 mm.";
        A.CallTo(() => _inputValidation.Validate(A<SpokeLengthCalculatorInput>._)).Returns(false);
        A.CallTo(() => _inputValidation.ErrorMessages).Returns(new Dictionary<string, string>
    {
        { "HubFlangeDiameterGearSideMm", errorMessage1 },
        { "RimInternalDiameterMm", errorMessage2 }
    });

        // Act
        _viewModel.HubFlangeDiameterGearSide.Value = 10; // Invalid
        _viewModel.RimInternalDiameter.Value = 100; // Invalid

        A.CallTo(() => _inputValidation.ErrorMessages).Returns(new Dictionary<string, string>
    {
        { "RimInternalDiameterMm", errorMessage2 }
    });
        _viewModel.HubFlangeDiameterGearSide.Value = 45; // Valid - corrected value
        _viewModel.RimInternalDiameter.Value = 600; // Valid

        // Assert - Only the corrected error is removed
        _viewModel.InputErrorMessages.Should().HaveCount(1);
        _viewModel.InputErrorMessages.Should().NotContain(msg => msg.Contains(errorMessage1));
        _viewModel.InputErrorMessages.Should().Contain(msg => msg.Contains(errorMessage2));
    }
}