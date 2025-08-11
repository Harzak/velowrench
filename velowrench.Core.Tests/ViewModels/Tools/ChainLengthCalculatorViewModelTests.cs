using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculs.Transmission.Chain;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Tools;
using velowrench.Utils.Enums;
using velowrench.Utils.Results;
using velowrench.Utils.EventArg;
using UnitsNet.Units;
using velowrench.Core.Units;
using velowrench.Core.Enums;

namespace velowrench.Core.Tests.ViewModels.Tools;

[TestClass]
public class ChainLengthCalculatorViewModelTests
{
    private ICalculFactory<ChainLengthCalculInput, ChainLengthCalculResult> _factory;
    private ICalcul<ChainLengthCalculInput, ChainLengthCalculResult> _calcul;
    private INavigationService _navigationService;
    private ILocalizer _localizer;
    private ChainLengthCalculatorViewModel _viewModel;

    [TestInitialize]
    public void Initialize()
    {
        _factory = A.Fake<ICalculFactory<ChainLengthCalculInput, ChainLengthCalculResult>>();
        _navigationService = A.Fake<INavigationService>();
        _localizer = A.Fake<ILocalizer>();
        _calcul = A.Fake<ICalcul<ChainLengthCalculInput, ChainLengthCalculResult>>();

        A.CallTo(() => _factory.CreateCalcul()).Returns(_calcul);

        _viewModel = new(_factory, _navigationService, _localizer);
    }

    [TestMethod]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Assert
        _viewModel.CanShowHelpPage.Should().BeTrue();
        _viewModel.CurrentState.Should().Be(ECalculState.NotStarted);
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
        OperationResult<ChainLengthCalculResult> expectedResult = new()
        {
            IsSuccess = true,
            Content = new ChainLengthCalculResult()
            {
                CalculatedAt = DateTime.UtcNow,
                ChainLengthInch = expectedChainLength,
                ChainLinks = expectedChainLinks,
                UsedInputs = new ChainLengthCalculInput()
                {
                    ChainStayLengthInch = 10,
                    TeethLargestChainring = 50,
                    TeethLargestSprocket = 30
                }
            }
        };

        A.CallTo(() => _calcul.Start(A<ChainLengthCalculInput>._)).Returns(expectedResult);
        A.CallTo(() => _calcul.State).Returns(ECalculState.NotStarted);

        // Act
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(10, LengthUnit.Inch);
        _viewModel.TeethLargestChainring = 50;
        _viewModel.TeethLargestSprocket = 30;

        // Assert
        _viewModel.RecommendedChainLength.Should().NotBeNull();
        _viewModel.RecommendedChainLength.Value.Should().Be(expectedChainLength);
        _viewModel.RecommendedChainLinks.Should().Be(expectedChainLinks);
        A.CallTo(() => _calcul.Start(A<ChainLengthCalculInput>._)).MustHaveHappenedOnceExactly();
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
        A.CallTo(() => _calcul.State).Returns(ECalculState.NotStarted);

        // Act
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Inch);
        _viewModel.TeethLargestChainring = 50;
        _viewModel.TeethLargestSprocket = 30;

        // Assert
        A.CallTo(() => _calcul.Start(A<ChainLengthCalculInput>._)).MustNotHaveHappened();
        _viewModel.CurrentState.Should().Be(ECalculState.NotStarted);
    }

    [TestMethod]
    public void OnInputsChanged_WithPartialInputs_ShouldNotStartCalculation()
    {
        // Arrange
        A.CallTo(() => _calcul.State).Returns(ECalculState.NotStarted);

        // Act
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(10, LengthUnit.Inch);
        _viewModel.TeethLargestChainring = 50;

        // Assert
        A.CallTo(() => _calcul.Start(A<ChainLengthCalculInput>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public void OnInputsChanged_WhenCalculationInProgress_ShouldNotStartNewCalculation()
    {
        // Arrange
        A.CallTo(() => _calcul.State).Returns(ECalculState.InProgress);

        // Act
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(10, LengthUnit.Inch);
        _viewModel.TeethLargestChainring = 50;
        _viewModel.TeethLargestSprocket = 30;

        // Assert
        A.CallTo(() => _calcul.Start(A<ChainLengthCalculInput>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public void Calculate_WithSameInputsAsPrevious_ShouldNotRecalculate()
    {
        // Arrange
        ChainLengthCalculInput input = new()
        {
            ChainStayLengthInch = 10,
            TeethLargestChainring = 50,
            TeethLargestSprocket = 30
        };

        OperationResult<ChainLengthCalculResult> result = new()
        {
            IsSuccess = true,
            Content = new ChainLengthCalculResult()
            {
                CalculatedAt = DateTime.UtcNow,
                ChainLengthInch = 15,
                ChainLinks = 101,
                UsedInputs = input
            }
        };

        A.CallTo(() => _calcul.Start(A<ChainLengthCalculInput>._)).Returns(result);
        A.CallTo(() => _calcul.State).Returns(ECalculState.NotStarted);

        // Act - First calculation
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(10, LengthUnit.Inch);
        _viewModel.TeethLargestChainring = 50;
        _viewModel.TeethLargestSprocket = 30;

        // Act - Second time with same inputs
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(10, LengthUnit.Inch);

        // Assert
        A.CallTo(() => _calcul.Start(A<ChainLengthCalculInput>._)).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void Calculate_WithFailedResult_ShouldNotUpdateResults()
    {
        // Arrange
        OperationResult<ChainLengthCalculResult> failedResult = new()
        {
            IsSuccess = false
        };

        A.CallTo(() => _calcul.Start(A<ChainLengthCalculInput>._)).Returns(failedResult);
        A.CallTo(() => _calcul.State).Returns(ECalculState.NotStarted);

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
        CalculStateEventArgs eventArgs = new(ECalculState.Failed);

        // Act
        _calcul.StateChanged += Raise.With(_calcul, eventArgs);

        // Assert
        _viewModel.CurrentState.Should().Be(ECalculState.Failed);
    }


    [TestMethod]
    public void ChainStayLengthValueChanged_ShouldTriggerInputValidation()
    {
        // Arrange
        A.CallTo(() => _calcul.State).Returns(ECalculState.NotStarted);
        ConvertibleDouble<LengthUnit> chainStayLength = new(15, LengthUnit.Inch);

        OperationResult<ChainLengthCalculResult> result = new()
        {
            IsSuccess = true,
            Content = new ChainLengthCalculResult()
            {
                CalculatedAt = DateTime.UtcNow,
                ChainLengthInch = 20,
                ChainLinks = 120,
                UsedInputs = new ChainLengthCalculInput()
                {
                    ChainStayLengthInch = 15,
                    TeethLargestChainring = 52,
                    TeethLargestSprocket = 28
                }
            }
        };
        A.CallTo(() => _calcul.Start(A<ChainLengthCalculInput>._)).Returns(result);

        // Act
        _viewModel.ChainStayLength = chainStayLength;
        _viewModel.TeethLargestChainring = 52;
        _viewModel.TeethLargestSprocket = 28;

        chainStayLength.Value = 16;

        // Assert
        A.CallTo(() => _calcul.Start(A<ChainLengthCalculInput>._)).MustHaveHappened();
    }

    [TestMethod]
    public void Calculate_WithUnitConversion_ShouldConvertCorrectly()
    {
        // Arrange
        OperationResult<ChainLengthCalculResult> expectedResult = new()
        {
            IsSuccess = true,
            Content = new ChainLengthCalculResult()
            {
                CalculatedAt = DateTime.UtcNow,
                ChainLengthInch = 42.0,
                ChainLinks = 84,
                UsedInputs = new ChainLengthCalculInput()
                {
                    ChainStayLengthInch = 17.72, // ~45cm converted to inches
                    TeethLargestChainring = 53,
                    TeethLargestSprocket = 32
                }
            }
        };

        A.CallTo(() => _calcul.Start(A<ChainLengthCalculInput>.That.Matches(input =>
            Math.Abs(input.ChainStayLengthInch - 17.72) < 0.1))).Returns(expectedResult);
        A.CallTo(() => _calcul.State).Returns(ECalculState.NotStarted);

        // Act
        _viewModel.ChainStayLength = new ConvertibleDouble<LengthUnit>(45, LengthUnit.Centimeter); // 45cm
        _viewModel.TeethLargestChainring = 53;
        _viewModel.TeethLargestSprocket = 32;

        // Assert
        _viewModel.RecommendedChainLength.Should().NotBeNull();
        _viewModel.RecommendedChainLength.Value.Should().Be(42.0);
        _viewModel.RecommendedChainLength.Unit.Should().Be(LengthUnit.Inch);
        _viewModel.RecommendedChainLinks.Should().Be(84);
    }
}
