using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using velowrench.Calculations.Calculators;
using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Interfaces;
using velowrench.Calculations.Validation.Results;
using velowrench.Core.Validation;
using velowrench.Core.Validation.Pipeline;
using velowrench.Utils.Enums;
using velowrench.Utils.EventArg;
using velowrench.Utils.Results;

namespace velowrench.Calculations.Tests.Calculs;

[TestClass]
public class BaseCalculTests
{
    private ILogger _logger;
    private ICalculatorInputValidator<TestInput> _inputValidator;
    private TestableCalculator _calculator;

    [TestInitialize]
    public void Initialize()
    {
        _logger = A.Fake<ILogger>();
        _inputValidator = A.Fake<ICalculatorInputValidator<TestInput>>();
        _calculator = new TestableCalculator(_inputValidator, _logger);

        A.CallTo(() => _inputValidator.ValidateWithResults(A<TestInput>._, A<ValidationContext?>._))
            .Returns(ValidationResult.WithSuccess());
    }

    [TestMethod]
    public void InitialState_ShouldBeNotStarted()
    {
        // Assert
        _calculator.State.Should().Be(ECalculatorState.NotStarted);
        _calculator.LastResult.Should().BeNull();
    }

    [TestMethod]
    public void Start_WithValidInput_ShouldTransitionToComputedState()
    {
        // Arrange
        TestInput input = new()
        {
            Value = 42
        };

        // Act
        OperationResult<TestResult> result = _calculator.Start(input);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _calculator.State.Should().Be(ECalculatorState.Computed);
        _calculator.LastResult.Should().NotBeNull();
        _calculator.LastResult.Value.Should().Be(42 * 2);
    }


    [TestMethod]
    public void Start_WhenCalculateReturnsFailure_ShouldTransitionToFailedState()
    {
        // Arrange
        TestInput input = new()
        {
            Value = -1
        };

        // Act
        OperationResult<TestResult> result = _calculator.Start(input);

        // Assert
        result.IsSuccess.Should().BeFalse();
        _calculator.State.Should().Be(ECalculatorState.Failed);
        _calculator.LastResult.Should().BeNull();
    }

    [TestMethod]
    public void Start_CanBeCalledMultipleTimes_AfterCompletion()
    {
        // Arrange
        TestInput input1 = new()
        {
            Value = 10
        };
        TestInput input2 = new()
        {
            Value = 20
        };

        // Act
        OperationResult<TestResult> result1 = _calculator.Start(input1);
        OperationResult<TestResult> result2 = _calculator.Start(input2);

        // Assert
        result1.IsSuccess.Should().BeTrue();
        result2.IsSuccess.Should().BeTrue();
        _calculator.State.Should().Be(ECalculatorState.Computed);
        _calculator.LastResult.Should().NotBeNull();
        _calculator.LastResult.Value.Should().Be(20 * 2);
    }

    [TestMethod]
    public void Start_CanBeCalledAfterFailure_ShouldRestart()
    {
        // Arrange
        TestInput invalidInput = new()
        {
            Value = -1
        };
        TestInput validInput = new()
        {
            Value = 15
        };

        // Act
        OperationResult<TestResult> result1 = _calculator.Start(invalidInput);
        OperationResult<TestResult> result2 = _calculator.Start(validInput);

        // Assert
        result1.IsSuccess.Should().BeFalse();
        result2.IsSuccess.Should().BeTrue();
        _calculator.State.Should().Be(ECalculatorState.Computed);
        _calculator.LastResult.Should().NotBeNull();
    }

    [TestMethod]
    public void Start_WithValidInput_ShouldTriggerStateChangedEvents()
    {
        // Arrange
        TestInput input = new()
        {
            Value = 42
        };
        List<ECalculatorState> stateChanges = [];

        _calculator.StateChanged += (sender, args) => stateChanges.Add(args.State);

        // Act
        OperationResult<TestResult> result = _calculator.Start(input);

        // Assert
        result.IsSuccess.Should().BeTrue();
        stateChanges.Should().HaveCount(2);
        stateChanges[0].Should().Be(ECalculatorState.InProgress);
        stateChanges[1].Should().Be(ECalculatorState.Computed);
    }

    [TestMethod]
    public void Start_WithFailingInput_ShouldTriggerFailedStateEvent()
    {
        // Arrange
        TestInput input = new() { Value = -1 };
        List<ECalculatorState> stateChanges = [];
        List<CalculatorStateEventArgs> eventArgs = [];

        _calculator.StateChanged += (sender, args) =>
        {
            stateChanges.Add(args.State);
            eventArgs.Add(args);
        };

        // Act
        OperationResult<TestResult> result = _calculator.Start(input);

        // Assert
        result.IsSuccess.Should().BeFalse();
        stateChanges.Should().HaveCount(2);
        stateChanges[0].Should().Be(ECalculatorState.InProgress);
        stateChanges[1].Should().Be(ECalculatorState.Failed);

        eventArgs.Should().HaveCount(2);
        eventArgs[0].State.Should().Be(ECalculatorState.InProgress);
        eventArgs[1].State.Should().Be(ECalculatorState.Failed);
    }
}

public class TestInput
{
    public int Value { get; set; }
}

public class TestResult
{
    public int Value { get; set; }
}

public class TestableCalculator : BaseCalculator<TestInput, TestResult>
{
    protected override string CalculatorName => "TestableCalcul";

    public override ICalculatorInputValidator<TestInput> InputValidator { get; }    

    public TestableCalculator(ICalculatorInputValidator<TestInput> inputValidation, ILogger logger) : base(logger)
    {
        this.InputValidator = inputValidation;  
    }

    protected override OperationResult<TestResult> Calculate(TestInput input)
    {
        var result = new OperationResult<TestResult>();

        if (input.Value < 0)
        {
            return result.WithError("Negative values not allowed");
        }

        result.Content = new TestResult { Value = input.Value * 2 };
        return result.WithSuccess();
    }
}