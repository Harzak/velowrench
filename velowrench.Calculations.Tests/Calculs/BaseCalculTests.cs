using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using velowrench.Calculations.Calculs;
using velowrench.Calculations.Exceptions;
using velowrench.Calculations.Interfaces;
using velowrench.Utils.Enums;
using velowrench.Utils.EventArg;
using velowrench.Utils.Results;

namespace velowrench.Calculations.Tests.Calculs;

[TestClass]
public class BaseCalculTests
{
    private ILogger _logger;
    private ICalculInputValidation<TestInput> _inputValidation;
    private TestableCalcul _calcul;

    [TestInitialize]
    public void Initialize()
    {
        _logger = A.Fake<ILogger>();
        _inputValidation = A.Fake<ICalculInputValidation<TestInput>>();
        _calcul = new TestableCalcul(() => _inputValidation, _logger);
    }

    [TestMethod]
    public void InitialState_ShouldBeNotStarted()
    {
        // Assert
        _calcul.State.Should().Be(ECalculState.NotStarted);
        _calcul.LastResult.Should().BeNull();
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
        OperationResult<TestResult> result = _calcul.Start(input);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _calcul.State.Should().Be(ECalculState.Computed);
        _calcul.LastResult.Should().NotBeNull();
        _calcul.LastResult.Value.Should().Be(42 * 2);
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
        OperationResult<TestResult> result = _calcul.Start(input);

        // Assert
        result.IsSuccess.Should().BeFalse();
        _calcul.State.Should().Be(ECalculState.Failed);
        _calcul.LastResult.Should().BeNull();
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
        OperationResult<TestResult> result1 = _calcul.Start(input1);
        OperationResult<TestResult> result2 = _calcul.Start(input2);

        // Assert
        result1.IsSuccess.Should().BeTrue();
        result2.IsSuccess.Should().BeTrue();
        _calcul.State.Should().Be(ECalculState.Computed);
        _calcul.LastResult.Should().NotBeNull();
        _calcul.LastResult.Value.Should().Be(20 * 2);
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
        OperationResult<TestResult> result1 = _calcul.Start(invalidInput);
        OperationResult<TestResult> result2 = _calcul.Start(validInput);

        // Assert
        result1.IsSuccess.Should().BeFalse();
        result2.IsSuccess.Should().BeTrue();
        _calcul.State.Should().Be(ECalculState.Computed);
        _calcul.LastResult.Should().NotBeNull();
    }

    [TestMethod]
    public void Start_WithValidInput_ShouldTriggerStateChangedEvents()
    {
        // Arrange
        TestInput input = new()
        {
            Value = 42
        };
        List<ECalculState> stateChanges = [];

        _calcul.StateChanged += (sender, args) => stateChanges.Add(args.State);

        // Act
        OperationResult<TestResult> result = _calcul.Start(input);

        // Assert
        result.IsSuccess.Should().BeTrue();
        stateChanges.Should().HaveCount(2);
        stateChanges[0].Should().Be(ECalculState.InProgress);
        stateChanges[1].Should().Be(ECalculState.Computed);
    }

    [TestMethod]
    public void Start_WithFailingInput_ShouldTriggerFailedStateEvent()
    {
        // Arrange
        TestInput input = new () { Value = -1 };
        List<ECalculState> stateChanges = [];
        List<CalculStateEventArgs> eventArgs = [];

        _calcul.StateChanged += (sender, args) =>
        {
            stateChanges.Add(args.State);
            eventArgs.Add(args);
        };

        // Act
        OperationResult<TestResult> result = _calcul.Start(input);

        // Assert
        result.IsSuccess.Should().BeFalse();
        stateChanges.Should().HaveCount(2);
        stateChanges[0].Should().Be(ECalculState.InProgress);
        stateChanges[1].Should().Be(ECalculState.Failed);

        eventArgs.Should().HaveCount(2);
        eventArgs[0].State.Should().Be(ECalculState.InProgress);
        eventArgs[1].State.Should().Be(ECalculState.Failed);
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

public class TestableCalcul : BaseCalcul<TestInput, TestResult>
{
    protected override string CalculName => "TestableCalcul";

    public TestableCalcul(Func<ICalculInputValidation<TestInput>> inputValidation, ILogger logger) : base(inputValidation, logger)
    {

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