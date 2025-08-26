using FakeItEasy;
using FluentAssertions;
using UnitsNet.Units;
using velowrench.Calculations.Calculators.Transmission.Gear;
using velowrench.Calculations.Enums;
using velowrench.Calculations.Interfaces;
using velowrench.Calculations.Validation.Results;
using velowrench.Core.Interfaces;
using velowrench.Core.Models;
using velowrench.Core.Validation;
using velowrench.Core.ViewModels.Tools;
using velowrench.Repository.Interfaces;
using velowrench.Repository.Models;
using velowrench.Utils.Enums;
using velowrench.Utils.EventArg;
using velowrench.Utils.Results;

namespace velowrench.Core.Tests.ViewModels.Tools;

[TestClass]
public class GearCalculatorViewModelTests
{
    private ICalculator<GearCalculatorInput, GearCalculatorResult> _calculator;
    private ICalculatorInputValidator<GearCalculatorInput> _inputValidation;
    private INavigationService _navigationService;
    private ILocalizer _localizer;
    private IComponentStandardRepository _repository;
    private ICalculatorFactory<GearCalculatorInput, GearCalculatorResult> _calculatorFactory;
    private IDebounceActionFactory _debounceActionFactory;
    private GearCalculatorViewModel _viewModel;

    [TestInitialize]
    public void Initialize()
    {
        _calculatorFactory = A.Fake<ICalculatorFactory<GearCalculatorInput, GearCalculatorResult>>();
        _debounceActionFactory = A.Fake<IDebounceActionFactory>();
        _navigationService = A.Fake<INavigationService>();
        _localizer = A.Fake<ILocalizer>();
        _repository = A.Fake<IComponentStandardRepository>();
        _calculator = A.Fake<ICalculator<GearCalculatorInput, GearCalculatorResult>>();
        _inputValidation = A.Fake<ICalculatorInputValidator<GearCalculatorInput>>();
    }

    private void GlobalSetup(ECalculatorState calculatorState, ValidationResult validation)
    {
        A.CallTo(() => _inputValidation.ValidateProperty(A<GearCalculatorInput>._, A<string>._, A<ValidationContext>._))
            .Returns(validation);
        A.CallTo(() => _inputValidation.ValidateWithResults(A<GearCalculatorInput>._, A<ValidationContext>._))
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
        List<WheelSpecificationModel> wheels =
        [
            new(label: "26'' (559) — common MTB/cruiser", bSDin:22.008, tyreHeightIn:2.25),
            new(label: "700C / 29'' (622)", bSDin:24.488, tyreHeightIn:1.05)
        ];

        List<CranksetSpecificationModel> cranks =
        [
            new("170 mm", 170),
            new("175 mm", 175),
        ];

        List<CadenceModel> cadences =
        [
            new("80 rpm", 80),
            new("90 rpm", 90),
        ];

        List<SprocketSpecificationModel> sprockets =
        [
            new("11", 11),
            new("13", 13),
            new("15", 15),
            new("17", 17),
            new("19", 19),
            new("22", 22),
            new("25", 25),
            new("28", 28),
            new("32", 32),
            new("36", 36)
        ];

        A.CallTo(() => _repository.GetMostCommonWheelSpecifications()).Returns(wheels);
        A.CallTo(() => _repository.GetAllCranksetSpecifications()).Returns(cranks);
        A.CallTo(() => _repository.GetAllCandences()).Returns(cadences);
        A.CallTo(() => _repository.GetMostCommonSprocketSpecifications()).Returns(sprockets);

        _viewModel = new(_calculatorFactory, _navigationService, _debounceActionFactory, _repository, _localizer);
    }

    [TestMethod]
    public void Calculate_WithValidInputs_ShouldUpdateResults()
    {
        // Arrange
        List<double> expectedValues = [58.9, 54.0, 49.8];
        OperationResult<GearCalculatorResult> expectedResult = new()
        {
            IsSuccess = true,
            Content = new GearCalculatorResult()
            {
                CalculatedAt = DateTime.UtcNow,
                ValuesLargeOrUniqueChainring = expectedValues,
                ValuesMediumChainring = null,
                ValuesSmallChainring = null,
                Unit = LengthUnit.Inch,
                UsedInputs = new GearCalculatorInput(sprockets: [11, 12, 13])
                {
                    CrankLengthMm = 170,
                    RevolutionPerMinute = 90,
                    CalculatorType = EGearCalculatorType.GearInches,
                    TeethNumberLargeOrUniqueChainring = 46,
                    TyreOuterDiameterIn = 27.0,
                    Precision = 2
                }
            }
        };

        A.CallTo(() => _calculator.Start(A<GearCalculatorInput>._)).Returns(expectedResult);
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());

        SelectibleModel<SprocketSpecificationModel> sprocket11 = _viewModel.SourceSprockets.First(s => s.Value.TeethCount == 11);
        SelectibleModel<SprocketSpecificationModel> sprocket28 = _viewModel.SourceSprockets.First(s => s.Value.TeethCount == 28);

        // Act
        _viewModel.Chainring1TeethCount = 46;
        sprocket11.IsSelected = true;
        sprocket28.IsSelected = true;
        _viewModel.SprocketSelectedCommand.Execute(sprocket11);

        // Assert
        _viewModel.GearCalculResultRows.Should().HaveCount(expectedValues.Count);
        _viewModel.GearCalculResultRows[0].ValueForChainring1.Should().Be(expectedValues[0]);
    }

    [TestMethod]
    public void OnInputsChanged_WithInvalidInputs_ShouldNotStartCalculation()
    {
        // Arrange
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithError("", ""));

        // Act
        _viewModel.Chainring1TeethCount = 0;
        _viewModel.SelectedCalculatorType = EGearCalculatorType.GearInches;

        // Assert
        A.CallTo(() => _calculator.Start(A<GearCalculatorInput>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public void OnInputsChanged_WhenCalculationInProgress_ShouldNotStartNewCalculation()
    {
        // Arrange
        this.GlobalSetup(ECalculatorState.InProgress, ValidationResult.WithSuccess());

        SelectibleModel<SprocketSpecificationModel> sprocket = _viewModel.SourceSprockets.First();

        // Act
        _viewModel.Chainring1TeethCount = 46;
        sprocket.IsSelected = true;
        _viewModel.SprocketSelectedCommand.Execute(sprocket);

        // Assert
        A.CallTo(() => _calculator.Start(A<GearCalculatorInput>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public void Calculate_WithSameInputsAsPrevious_ShouldNotRecalculate()
    {
        // Arrange
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        Fake.ClearRecordedCalls(_calculator);

        // Act
        _viewModel.Chainring1TeethCount = 46;
        _viewModel.Chainring1TeethCount = 46;

        // Assert 
        A.CallTo(() => _calculator.Start(A<GearCalculatorInput>._)).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void Calculate_WithFailedResult_ShouldNotUpdateResults()
    {
        // Arrange
        OperationResult<GearCalculatorResult> failedResult = new()
        {
            IsSuccess = false
        };

        A.CallTo(() => _calculator.Start(A<GearCalculatorInput>._)).Returns(failedResult);
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());

        SelectibleModel<SprocketSpecificationModel> sprocket = _viewModel.SourceSprockets.First();

        // Act
        _viewModel.Chainring1TeethCount = 46;
        sprocket.IsSelected = true;
        _viewModel.SprocketSelectedCommand.Execute(sprocket);

        // Assert
        _viewModel.GearCalculResultRows.Should().BeEmpty();
    }

    [TestMethod]
    public void OnGearCalculatorStateChanged_DirectStateChange_ShouldUpdateImmediately()
    {
        // Arrange
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        CalculatorStateEventArgs eventArgs = new(ECalculatorState.Failed);

        // Act
        _calculator.StateChanged += Raise.With(_calculator, eventArgs);

        // Assert
        _viewModel.CurrentState.Should().Be(ECalculatorState.Failed);
    }

    [TestMethod]
    public void SelectedCalculatorType_Changed_ShouldTriggerCalculation()
    {
        // Arrange
        OperationResult<GearCalculatorResult> result = new()
        {
            IsSuccess = true,
            Content = new GearCalculatorResult()
            {
                CalculatedAt = DateTime.UtcNow,
                ValuesLargeOrUniqueChainring = [5.5],
                Unit = SpeedUnit.MeterPerSecond,
                UsedInputs = new GearCalculatorInput(sprockets: [11])
                {
                    CalculatorType = EGearCalculatorType.Speed,
                    TyreOuterDiameterIn = 27.0,
                    CrankLengthMm = 170,
                    TeethNumberLargeOrUniqueChainring = 46,
                    RevolutionPerMinute = 90
                }
            }
        };

        A.CallTo(() => _calculator.Start(A<GearCalculatorInput>._)).Returns(result);
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());

        SelectibleModel<SprocketSpecificationModel> sprocket = _viewModel.SourceSprockets.First();
        sprocket.IsSelected = true;

        // Act
        _viewModel.SelectedCalculatorType = EGearCalculatorType.Speed;

        // Assert
        A.CallTo(() => _calculator.Start(A<GearCalculatorInput>.That.Matches(
            input => input.CalculatorType == EGearCalculatorType.Speed))).MustHaveHappened();
    }

    [TestMethod]
    public void SelectedWheel_Changed_ShouldTriggerCalculation()
    {
        // Arrange
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        WheelSpecificationModel newWheel = new(label: "700C / 29'' (622)", bSDin: 24.488, tyreHeightIn: 1.05);

        // Act
        _viewModel.SelectedWheel = newWheel;

        // Assert
        A.CallTo(() => _calculator.Start(A<GearCalculatorInput>._)).MustHaveHappened();
    }

    [TestMethod]
    public void SelectedCrank_Changed_ShouldTriggerCalculation()
    {
        // Arrange
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        CranksetSpecificationModel newCrank = new("175 mm", 175);

        // Act
        _viewModel.SelectedCrank = newCrank;

        // Assert
        A.CallTo(() => _calculator.Start(A<GearCalculatorInput>._)).MustHaveHappened();
    }

    [TestMethod]
    public void SelectedCadence_Changed_ShouldTriggerCalculation()
    {
        // Arrange
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        CadenceModel newCadence = new("90 rpm", 90);

        // Act
        _viewModel.SelectedCadence = newCadence;

        // Assert
        A.CallTo(() => _calculator.Start(A<GearCalculatorInput>._)).MustHaveHappened();
    }

    [TestMethod]
    public void SprocketSelected_ShouldUpdateSelectedSprocketsStr()
    {
        // Arrange
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        SelectibleModel<SprocketSpecificationModel> sprocket11 = _viewModel.SourceSprockets.First(s => s.Value.TeethCount == 11);
        SelectibleModel<SprocketSpecificationModel> sprocket28 = _viewModel.SourceSprockets.First(s => s.Value.TeethCount == 28);

        // Act
        sprocket11.IsSelected = true;
        sprocket28.IsSelected = true;
        _viewModel.SprocketSelectedCommand.Execute(sprocket11);

        // Assert
        _viewModel.SelectedSprocketsStr.Should().Contain("11");
        _viewModel.SelectedSprocketsStr.Should().Contain("28");
    }

    [TestMethod]
    public void Calculate_WithMultipleChainrings_ShouldPopulateAllColumns()
    {
        // Arrange
        List<double> valuesChainring1 = [112.9, 103.5];
        List<double> valuesChainring2 = [83.5, 76.5];
        List<double> valuesChainring3 = [58.9, 54.0];

        OperationResult<GearCalculatorResult> expectedResult = new()
        {
            IsSuccess = true,
            Content = new GearCalculatorResult()
            {
                CalculatedAt = DateTime.UtcNow,
                ValuesLargeOrUniqueChainring = valuesChainring1,
                ValuesMediumChainring = valuesChainring2,
                ValuesSmallChainring = valuesChainring3,
                Unit = LengthUnit.Inch,
                UsedInputs = new GearCalculatorInput(sprockets: [11, 12])
                {
                    CalculatorType = EGearCalculatorType.GearInches,
                    CrankLengthMm = 170,
                    RevolutionPerMinute = 90,
                    TeethNumberLargeOrUniqueChainring = 46,
                    TeethNumberMediumChainring = 34,
                    TeethNumberSmallChainring = 24,
                    TyreOuterDiameterIn = 27.0,
                    Precision = 2
                }
            }
        };

        A.CallTo(() => _calculator.Start(A<GearCalculatorInput>._)).Returns(expectedResult);
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());

        SelectibleModel<SprocketSpecificationModel> sprocket11 = _viewModel.SourceSprockets.First(s => s.Value.TeethCount == 11);
        SelectibleModel<SprocketSpecificationModel> sprocket28 = _viewModel.SourceSprockets.First(s => s.Value.TeethCount == 28);

        // Act
        _viewModel.Chainring1TeethCount = 46;
        _viewModel.Chainring2TeethCount = 34;
        _viewModel.Chainring3TeethCount = 24;
        sprocket11.IsSelected = true;
        sprocket28.IsSelected = true;
        _viewModel.SprocketSelectedCommand.Execute(sprocket11);

        // Assert
        _viewModel.GearCalculResultRows.Should().HaveCount(2);
        _viewModel.GearCalculResultRows[0].ValueForChainring1.Should().Be(valuesChainring1[0]);
        _viewModel.GearCalculResultRows[0].ValueForChainring2.Should().Be(valuesChainring2[0]);
        _viewModel.GearCalculResultRows[0].ValueForChainring3.Should().Be(valuesChainring3[0]);
        _viewModel.GearCalculResultRows[1].ValueForChainring1.Should().Be(valuesChainring1[1]);
        _viewModel.GearCalculResultRows[1].ValueForChainring2.Should().Be(valuesChainring2[1]);
        _viewModel.GearCalculResultRows[1].ValueForChainring3.Should().Be(valuesChainring3[1]);
    }

    [TestMethod]
    public void SelectedResultUnit_Changed_ShouldUpdateAllResultRows()
    {
        // Arrange
        OperationResult<GearCalculatorResult> expectedResult = new()
        {
            IsSuccess = true,
            Content = new GearCalculatorResult()
            {
                CalculatedAt = DateTime.UtcNow,
                ValuesLargeOrUniqueChainring = [112.9],
                Unit = LengthUnit.Inch,
                UsedInputs = new GearCalculatorInput(sprockets: [11])
                {
                    CalculatorType = EGearCalculatorType.GearInches,
                    CrankLengthMm = 170,
                    RevolutionPerMinute = 90,
                    TeethNumberLargeOrUniqueChainring = 46,
                    TyreOuterDiameterIn = 27.0,
                    Precision = 2
                }
            }
        };

        A.CallTo(() => _calculator.Start(A<GearCalculatorInput>._)).Returns(expectedResult);
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());

        SelectibleModel<SprocketSpecificationModel> sprocket = _viewModel.SourceSprockets.First();
        sprocket.IsSelected = true;

        // Setup available units
        _viewModel.AvailableResultUnits.Add(LengthUnit.Inch);
        _viewModel.AvailableResultUnits.Add(LengthUnit.Centimeter);

        // Act - First trigger calculation to populate result rows
        _viewModel.SprocketSelectedCommand.Execute(sprocket);

        // Act - Change result unit
        _viewModel.SelectedResultUnit = LengthUnit.Centimeter;

        // Assert
        _viewModel.GearCalculResultRows.Should().HaveCount(1);
        _viewModel.GearCalculResultRows[0].ValueUnit.Should().Be(LengthUnit.Centimeter);
    }

    [TestMethod]
    public void ValidationBehavior_InitialState_ShouldHaveNoErrors()
    {
        // Arrange & Act
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());

        // Assert
        _viewModel.InputErrorMessages.Should().BeEmpty();
        _viewModel.CurrentState.Should().Be(ECalculatorState.NotStarted);
    }

    [TestMethod]
    public void ValidationBehavior_UserEntersInvalidValue_ShouldShowError()
    {
        // Arrange
        string errorMessage = "Chainring teeth count must be between 10 and 120.";
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithError("TeethNumberLargeOrUniqueChainring", errorMessage));

        // Act
        _viewModel.Chainring1TeethCount = 5; // Below minimum

        // Assert
        _viewModel.InputErrorMessages.Should().NotBeEmpty();
        _viewModel.InputErrorMessages.Should().Contain(msg => msg.Contains(errorMessage));
    }

    [TestMethod]
    public void ValidationBehavior_MultipleInvalidInputs_ShouldShowCumulativeErrors()
    {
        // Arrange
        string errorMessage1 = "Chainring teeth count must be between 10 and 120.";
        string errorMessage2 = "Tyre outer diameter must be between 7 and 38 inches.";
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithErrors([
            new ValidationError()
            {
                PropertyName = "TeethNumberLargeOrUniqueChainring",
                Message = errorMessage1
            },
            new ValidationError()
            {
                PropertyName = "TyreOuterDiameterIn",
                Message = errorMessage2
            },
        ]));

        // Act 
        _viewModel.Chainring1TeethCount = 5; // Below minimum
        _viewModel.SelectedWheel = new WheelSpecificationModel("Invalid wheel", 3.0, 1.0); // Below minimum diameter

        // Assert
        _viewModel.InputErrorMessages.Should().HaveCount(2);
        _viewModel.InputErrorMessages.Should().Contain(msg => msg.Contains(errorMessage1));
        _viewModel.InputErrorMessages.Should().Contain(msg => msg.Contains(errorMessage2));
    }
}