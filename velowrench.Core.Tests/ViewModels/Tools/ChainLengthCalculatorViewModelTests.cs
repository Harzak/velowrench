using FakeItEasy;
using FluentAssertions;
using UnitsNet.Units;
using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Interfaces;
using velowrench.Calculations.Validation;
using velowrench.Calculations.Validation.Results;
using velowrench.Core.Interfaces;
using velowrench.Core.Units;
using velowrench.Core.ViewModels.Tools;
using velowrench.Utils.Enums;
using velowrench.Utils.EventArg;
using velowrench.Utils.Interfaces;
using velowrench.Utils.Results;

namespace velowrench.Core.Tests.ViewModels.Tools;

[TestClass]
public class ChainLengthCalculatorViewModelTests
{
    private ICalculator<ChainLengthCalculatorInput, ChainLengthCalculatorResult> _calculator;
    private ICalculatorInputValidator<ChainLengthCalculatorInput> _inputValidation;
    private INavigationService _navigationService;
    private ILocalizer _localizer;
    private ICalculatorFactory<ChainLengthCalculatorInput, ChainLengthCalculatorResult> _calculatorFactory;
    private IUnitStore _unitStore;
    private IToolbar _toolbar;
    private IClipboardInterop _clipboardInterop;
    private IDebounceActionFactory _debounceActionFactory;
    private ChainLengthCalculatorViewModel _viewModel;

    [TestInitialize]
    public void TestInitialize()
    {
        _calculatorFactory = A.Fake<ICalculatorFactory<ChainLengthCalculatorInput, ChainLengthCalculatorResult>>();
        _debounceActionFactory = A.Fake<IDebounceActionFactory>();
        _navigationService = A.Fake<INavigationService>();
        _localizer = A.Fake<ILocalizer>();
        _calculator = A.Fake<ICalculator<ChainLengthCalculatorInput, ChainLengthCalculatorResult>>();
        _inputValidation = A.Fake<ICalculatorInputValidator<ChainLengthCalculatorInput>>();
        _unitStore = A.Fake<IUnitStore>();
        _toolbar = A.Fake<IToolbar>();
        _clipboardInterop = A.Fake<IClipboardInterop>();
    }

    private void GlobalSetup(ECalculatorState calculatorState, ValidationResult validation)
    {
        A.CallTo(() => _inputValidation.ValidateProperty(A<ChainLengthCalculatorInput>._, A<string>._, A<ValidationContext>._))
            .Returns(validation);
        A.CallTo(() => _inputValidation.ValidateWithResults(A<ChainLengthCalculatorInput>._, A<ValidationContext>._))
            .Returns(validation);

        A.CallTo(() => _calculator.InputValidator)
            .Returns(_inputValidation);
        A.CallTo(() => _calculator.State)
            .Returns(calculatorState);

        A.CallTo(() => _calculatorFactory.CreateCalculator())
            .Returns(_calculator);

        A.CallTo(() => _debounceActionFactory.CreateDebounceUIAction(A<Action>._, A<int>._))
            .ReturnsLazily((Action action, int delayMs) => new TestDebounceAction(action));

        _viewModel = new(_calculatorFactory, _unitStore, _navigationService, _debounceActionFactory, _localizer, _toolbar, _clipboardInterop);
    }


    [TestMethod]
    public async Task Calculate_WithValidInputs_ShouldUpdateResults()
    {
        // Arrange
        double expectedChainLength = 15;
        int expectedChainLinks = 101;
        OperationResult<ChainLengthCalculatorResult> expectedResult = new()
        {
            IsSuccess = true,
            Content = new ChainLengthCalculatorResult()
            {
                CalculatedAt = DateTime.UtcNow,
                ChainLengthIn =  expectedChainLength,
                ChainLinks = expectedChainLinks,
                UsedInputs = new ChainLengthCalculatorInput()
                {
                    ChainStayLengthIn = 10,
                    TeethLargestChainring = 50,
                    TeethLargestSprocket = 30
                }
            }
        };

        A.CallTo(() => _calculator.Start(A<ChainLengthCalculatorInput>._)).Returns(expectedResult);
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Act
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(10, LengthUnit.Inch);
        _viewModel.TeethLargestChainring = 50;
        _viewModel.TeethLargestSprocket = 30;

        // Assert
        _viewModel.RecommendedChainLength.Should().NotBeNull();
        _viewModel.RecommendedChainLength.Value.Should().Be(expectedChainLength);
        _viewModel.RecommendedChainLinks.Should().Be(expectedChainLinks);
    }

    [TestMethod]
    public async Task OnInputsChanged_WithInvalidInputs_ShouldNotStartCalculation()
    {
        // Arrange
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithError("ChainStayLength", "invalid"));
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Act
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Inch);
        _viewModel.TeethLargestChainring = 50;
        _viewModel.TeethLargestSprocket = 30;

        // Assert
        A.CallTo(() => _calculator.Start(A<ChainLengthCalculatorInput>._)).MustNotHaveHappened();
        _viewModel.CurrentState.Should().Be(ECalculatorState.NotStarted);
    }

    [TestMethod]
    public async Task OnInputsChanged_WhenCalculationInProgress_ShouldNotStartNewCalculation()
    {
        // Arrange
        this.GlobalSetup(ECalculatorState.InProgress, ValidationResult.WithSuccess());
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Act
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(10, LengthUnit.Inch);
        _viewModel.TeethLargestChainring = 50;
        _viewModel.TeethLargestSprocket = 30;

        // Assert
        A.CallTo(() => _calculator.Start(A<ChainLengthCalculatorInput>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public async Task Calculate_WithSameInputsAsPrevious_ShouldNotRecalculate()
    {
        // Arrange
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);
        Fake.ClearRecordedCalls(_calculator);

        // Act
        _viewModel.ChainStayLength!.Value = 10;
        _viewModel.ChainStayLength.Value = 10;

        // Assert
        A.CallTo(() => _calculator.Start(A<ChainLengthCalculatorInput>._)).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public async Task Calculate_WithFailedResult_ShouldNotUpdateResults()
    {
        // Arrange
        OperationResult<ChainLengthCalculatorResult> failedResult = new()
        {
            IsSuccess = false
        };

        A.CallTo(() => _calculator.Start(A<ChainLengthCalculatorInput>._))
            .Returns(failedResult);
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Act
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(10, LengthUnit.Inch);
        _viewModel.TeethLargestChainring = 50;
        _viewModel.TeethLargestSprocket = 30;

        // Assert
        _viewModel.RecommendedChainLength.Should().BeNull();
        _viewModel.RecommendedChainLinks.Should().Be(0);
    }

    [TestMethod]
    public async Task OnChainLengthCalculStateChanged_DirectStateChange_ShouldUpdateImmediately()
    {
        // Arrange
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        CalculatorStateEventArgs eventArgs = new(ECalculatorState.Failed);
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Act
        _calculator.StateChanged += Raise.With(_calculator, eventArgs);

        // Assert
        _viewModel.CurrentState.Should().Be(ECalculatorState.Failed);
    }


    [TestMethod]
    public async Task ChainStayLengthValueChanged_ShouldTriggerInputValidation()
    {
        // Arrange
        ConvertibleDouble<LengthUnit> chainStayLength = new(15, LengthUnit.Inch);

        OperationResult<ChainLengthCalculatorResult> result = new()
        {
            IsSuccess = true,
            Content = new ChainLengthCalculatorResult()
            {
                CalculatedAt = DateTime.UtcNow,
                ChainLengthIn = 20,
                ChainLinks = 120,
                UsedInputs = new ChainLengthCalculatorInput()
                {
                    ChainStayLengthIn = 15,
                    TeethLargestChainring = 52,
                    TeethLargestSprocket = 28
                }
            }
        };
        A.CallTo(() => _calculator.Start(A<ChainLengthCalculatorInput>._)).Returns(result);
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Act
        _viewModel.ChainStayLength = chainStayLength;
        _viewModel.TeethLargestChainring = 52;
        _viewModel.TeethLargestSprocket = 28;

        chainStayLength.Value = 16;

        // Assert
        A.CallTo(() => _calculator.Start(A<ChainLengthCalculatorInput>._)).MustHaveHappened();
    }

    [TestMethod]
    public async Task Calculate_WithUnitConversion_ShouldConvertCorrectly()
    {
        // Arrange
        OperationResult<ChainLengthCalculatorResult> expectedResult = new()
        {
            IsSuccess = true,
            Content = new ChainLengthCalculatorResult()
            {
                CalculatedAt = DateTime.UtcNow,
                ChainLengthIn = 42.0,
                ChainLinks = 84,
                UsedInputs = new ChainLengthCalculatorInput()
                {
                    ChainStayLengthIn = 17.72,
                    TeethLargestChainring = 53,
                    TeethLargestSprocket = 32
                }
            }
        };

        A.CallTo(() => _calculator.Start(A<ChainLengthCalculatorInput>.That.Matches(input =>
            Math.Abs(input.ChainStayLengthIn - 17.72) < 0.1))).Returns(expectedResult);
        A.CallTo(() => _unitStore.LengthDefaultUnit).Returns(LengthUnit.Inch);
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithSuccess());
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Act
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(45, LengthUnit.Centimeter);
        _viewModel.TeethLargestChainring = 53;
        _viewModel.TeethLargestSprocket = 32;

        // Assert
        _viewModel.RecommendedChainLength.Should().NotBeNull();
        _viewModel.RecommendedChainLength.Value.Should().Be(42.0);
        _viewModel.RecommendedChainLength.Unit.Should().Be(LengthUnit.Inch);
        _viewModel.RecommendedChainLinks.Should().Be(84);
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
        string errorMessage = "Chainstay length must be greater than 1 inch.";
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithError("ChainStayLengthIn", errorMessage));
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Act
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Inch);

        // Assert
        _viewModel.InputErrorMessages.Should().NotBeEmpty();
        _viewModel.InputErrorMessages.Should().Contain(msg => msg.Contains(errorMessage));
    }

    [TestMethod]
    public async Task ValidationBehavior_MultipleInvalidInputs_ShouldShowCumulativeErrors()
    {
        // Arrange
        string errorMessage1 = "Chainstay length must be greater than 1 inch.";
        string errorMessage2 = "Largest chainring teeth must be between 10 and 120.";
        this.GlobalSetup(ECalculatorState.NotStarted, ValidationResult.WithErrors([
            new ValidationError()
            {
                PropertyName = "ChainStayLengthIn",
                Message = errorMessage1
            },
            new ValidationError()
            {
                PropertyName = "TeethLargestChainring",
                Message = errorMessage2
            },
        ]));
        await _viewModel.OnInitializedAsync().ConfigureAwait(false);

        // Act 
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Inch);
        _viewModel.TeethLargestChainring = 5;

        // Assert
        _viewModel.InputErrorMessages.Should().HaveCount(2);
        _viewModel.InputErrorMessages.Should().Contain(msg => msg.Contains(errorMessage1));
        _viewModel.InputErrorMessages.Should().Contain(msg => msg.Contains(errorMessage2));
    }
}


// Test helper class
public sealed class TestDebounceAction : IDebounceAction
{
    private readonly Action _action;

    public TestDebounceAction(Action action)
    {
        _action = action;
    }

    public void Execute()
    {
        _action();
    }

    public void Dispose() { }
}