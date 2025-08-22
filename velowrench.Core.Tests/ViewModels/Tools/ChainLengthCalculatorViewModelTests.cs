using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Tools;
using velowrench.Utils.Enums;
using velowrench.Utils.Results;
using velowrench.Utils.EventArg;
using UnitsNet.Units;
using velowrench.Core.Enums;
using velowrench.Calculations.Units;

namespace velowrench.Core.Tests.ViewModels.Tools;

[TestClass]
public class ChainLengthCalculatorViewModelTests
{
    private ICalculator<ChainLengthCalculatorInput, ChainLengthCalculatorResult> _calculator;
    private ICalculatorInputValidation<ChainLengthCalculatorInput> _inputValidation;
    private INavigationService _navigationService;
    private ILocalizer _localizer;
    private ChainLengthCalculatorViewModel _viewModel;

    [TestInitialize]
    public void Initialize()
    {
        var calculFactory = A.Fake<ICalculatorFactory<ChainLengthCalculatorInput, ChainLengthCalculatorResult>>();
        var actionFactory = A.Fake<IDebounceActionFactory>();
        _navigationService = A.Fake<INavigationService>();
        _localizer = A.Fake<ILocalizer>();
        _calculator = A.Fake<ICalculator<ChainLengthCalculatorInput, ChainLengthCalculatorResult>>();
        _inputValidation = A.Fake<ICalculatorInputValidation<ChainLengthCalculatorInput>>();

        A.CallTo(() => calculFactory.CreateCalculator()).Returns(_calculator);
        A.CallTo(() => _calculator.GetValidation()).Returns(_inputValidation);

        A.CallTo(() => actionFactory.CreateDebounceUIAction(A<Action>._, A<int>._))
        .ReturnsLazily((Action action, int delayMs) => 
        {
            return new TestDebounceAction(action);
        });

        _viewModel = new(calculFactory, _navigationService, actionFactory, _localizer);
    }

    [TestMethod]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Assert
        _viewModel.CanShowHelpPage.Should().BeTrue();
        _viewModel.CurrentState.Should().Be(ECalculatorState.NotStarted);
        _viewModel.ChainStayLength.Should().NotBeNull();
        _viewModel.ChainStayLength.Value.Should().Be(0);
        _viewModel.ChainStayLength.Unit.Should().Be(LengthUnit.Centimeter);
        _viewModel.RecommendedChainLinks.Should().Be(0);
        _viewModel.RecommendedChainLength.Should().BeNull();
    }


    [TestMethod]
    public void Calculate_WithValidInputs_ShouldUpdateResults()
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
                ChainLength =  new ConvertibleDouble<LengthUnit>(expectedChainLength, LengthUnit.Inch),
                ChainLinks = expectedChainLinks,
                UsedInputs = new ChainLengthCalculatorInput()
                {
                    ChainStayLength = new ConvertibleDouble<LengthUnit>(10, LengthUnit.Inch),
                    TeethLargestChainring = 50,
                    TeethLargestSprocket = 30
                }
            }
        };

        A.CallTo(() => _calculator.Start(A<ChainLengthCalculatorInput>._)).Returns(expectedResult);
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.NotStarted);
        A.CallTo(() => _inputValidation.Validate(A<ChainLengthCalculatorInput>._)).Returns(true);

        // Act
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(10, LengthUnit.Inch);
        _viewModel.TeethLargestChainring = 50;
        _viewModel.TeethLargestSprocket = 30;

        // Assert
        _viewModel.RecommendedChainLength.Should().NotBeNull();
        _viewModel.RecommendedChainLength.Value.Should().Be(expectedChainLength);
        _viewModel.RecommendedChainLinks.Should().Be(expectedChainLinks);
        A.CallTo(() => _calculator.Start(A<ChainLengthCalculatorInput>._)).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void TeethLargestChainring_WithInvalidValue_ShouldSetValidationError()
    {
        // Act
        _viewModel.TeethLargestChainring = 150;

        // Assert
        _viewModel.HasErrors.Should().BeTrue();
        _viewModel.GetErrors(nameof(_viewModel.TeethLargestChainring)).Should().NotBeEmpty();
    }

    [TestMethod]
    public void TeethLargestSprocket_WithInvalidValue_ShouldSetValidationError()
    {
        // Act
        _viewModel.TeethLargestSprocket = 80;

        // Assert
        _viewModel.HasErrors.Should().BeTrue();
        _viewModel.GetErrors(nameof(_viewModel.TeethLargestSprocket)).Should().NotBeEmpty();
    }

    [TestMethod]
    public void OnInputsChanged_WithInvalidInputs_ShouldNotStartCalculation()
    {
        // Arrange
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.NotStarted);

        // Act
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Inch);
        _viewModel.TeethLargestChainring = 50;
        _viewModel.TeethLargestSprocket = 30;

        // Assert
        A.CallTo(() => _calculator.Start(A<ChainLengthCalculatorInput>._)).MustNotHaveHappened();
        _viewModel.CurrentState.Should().Be(ECalculatorState.NotStarted);
    }

    [TestMethod]
    public void OnInputsChanged_WithPartialInputs_ShouldNotStartCalculation()
    {
        // Arrange
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.NotStarted);

        // Act
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(10, LengthUnit.Inch);
        _viewModel.TeethLargestChainring = 50;

        // Assert
        A.CallTo(() => _calculator.Start(A<ChainLengthCalculatorInput>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public void OnInputsChanged_WhenCalculationInProgress_ShouldNotStartNewCalculation()
    {
        // Arrange
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.InProgress);

        // Act
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(10, LengthUnit.Inch);
        _viewModel.TeethLargestChainring = 50;
        _viewModel.TeethLargestSprocket = 30;

        // Assert
        A.CallTo(() => _calculator.Start(A<ChainLengthCalculatorInput>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public void Calculate_WithSameInputsAsPrevious_ShouldNotRecalculate()
    {
        // Arrange
        ChainLengthCalculatorInput input = new()
        {
            ChainStayLength = new ConvertibleDouble<LengthUnit>(10, LengthUnit.Inch),
            TeethLargestChainring = 50,
            TeethLargestSprocket = 30
        };

        OperationResult<ChainLengthCalculatorResult> result = new()
        {
            IsSuccess = true,
            Content = new ChainLengthCalculatorResult()
            {
                CalculatedAt = DateTime.UtcNow,
                ChainLength = new ConvertibleDouble<LengthUnit>(15, LengthUnit.Inch),
                ChainLinks = 101,
                UsedInputs = input
            }
        };

        A.CallTo(() => _calculator.Start(A<ChainLengthCalculatorInput>._)).Returns(result);
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.NotStarted);
        A.CallTo(() => _inputValidation.Validate(A<ChainLengthCalculatorInput>._)).Returns(true);

        // Act - First calculation
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(10, LengthUnit.Inch);
        _viewModel.TeethLargestChainring = 50;
        _viewModel.TeethLargestSprocket = 30;

        // Act - Second time with same inputs
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(10, LengthUnit.Inch);

        // Assert
        A.CallTo(() => _calculator.Start(A<ChainLengthCalculatorInput>._)).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void Calculate_WithFailedResult_ShouldNotUpdateResults()
    {
        // Arrange
        OperationResult<ChainLengthCalculatorResult> failedResult = new()
        {
            IsSuccess = false
        };

        A.CallTo(() => _calculator.Start(A<ChainLengthCalculatorInput>._)).Returns(failedResult);
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.NotStarted);

        // Act
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(10, LengthUnit.Inch);
        _viewModel.TeethLargestChainring = 50;
        _viewModel.TeethLargestSprocket = 30;

        // Assert
        _viewModel.RecommendedChainLength.Should().BeNull();
        _viewModel.RecommendedChainLinks.Should().Be(0);
    }

    [TestMethod]
    public void OnChainLengthCalculStateChanged_DirectStateChange_ShouldUpdateImmediately()
    {
        // Arrange
        CalculatorStateEventArgs eventArgs = new(ECalculatorState.Failed);

        // Act
        _calculator.StateChanged += Raise.With(_calculator, eventArgs);

        // Assert
        _viewModel.CurrentState.Should().Be(ECalculatorState.Failed);
    }


    [TestMethod]
    public void ChainStayLengthValueChanged_ShouldTriggerInputValidation()
    {
        // Arrange
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.NotStarted);
        ConvertibleDouble<LengthUnit> chainStayLength = new(15, LengthUnit.Inch);

        OperationResult<ChainLengthCalculatorResult> result = new()
        {
            IsSuccess = true,
            Content = new ChainLengthCalculatorResult()
            {
                CalculatedAt = DateTime.UtcNow,
                ChainLength = new ConvertibleDouble<LengthUnit>(20, LengthUnit.Inch),
                ChainLinks = 120,
                UsedInputs = new ChainLengthCalculatorInput()
                {
                    ChainStayLength = new ConvertibleDouble<LengthUnit>(15, LengthUnit.Inch),
                    TeethLargestChainring = 52,
                    TeethLargestSprocket = 28
                }
            }
        };
        A.CallTo(() => _calculator.Start(A<ChainLengthCalculatorInput>._)).Returns(result);
        A.CallTo(() => _inputValidation.Validate(A<ChainLengthCalculatorInput>._)).Returns(true);


        // Act
        _viewModel.ChainStayLength = chainStayLength;
        _viewModel.TeethLargestChainring = 52;
        _viewModel.TeethLargestSprocket = 28;

        chainStayLength.Value = 16;

        // Assert
        A.CallTo(() => _calculator.Start(A<ChainLengthCalculatorInput>._)).MustHaveHappened();
    }

    [TestMethod]
    public void Calculate_WithUnitConversion_ShouldConvertCorrectly()
    {
        // Arrange
        OperationResult<ChainLengthCalculatorResult> expectedResult = new()
        {
            IsSuccess = true,
            Content = new ChainLengthCalculatorResult()
            {
                CalculatedAt = DateTime.UtcNow,
                ChainLength = new ConvertibleDouble<LengthUnit>(42.0, LengthUnit.Inch),
                ChainLinks = 84,
                UsedInputs = new ChainLengthCalculatorInput()
                {
                    ChainStayLength = new ConvertibleDouble<LengthUnit>(17.72, LengthUnit.Inch),
                    TeethLargestChainring = 53,
                    TeethLargestSprocket = 32
                }
            }
        };

        A.CallTo(() => _calculator.Start(A<ChainLengthCalculatorInput>.That.Matches(input =>
            Math.Abs(input.ChainStayLength.GetValueIn(LengthUnit.Inch) - 17.72) < 0.1))).Returns(expectedResult);
        A.CallTo(() => _calculator.State).Returns(ECalculatorState.NotStarted);
        A.CallTo(() => _inputValidation.Validate(A<ChainLengthCalculatorInput>._)).Returns(true);

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