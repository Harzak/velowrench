using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Constants;
using velowrench.Calculations.Exceptions;
using velowrench.Calculations.Interfaces;
using velowrench.Utils.Results;

namespace velowrench.Calculations.Tests.Calculs.Transmission.Chain;


[TestClass]
public class ChainLengthCalculTests
{
    private ILogger _logger;
    private ICalculatorInputValidation<ChainLengthCalculatorInput> _inputValidation;
    private ChainLengthCalculator _calculator;

    [TestInitialize]
    public void Initialize()
    {
        _logger = A.Fake<ILogger>();
        _inputValidation = new ChainLengthCalculatorInputValidator();
        _calculator = new ChainLengthCalculator(() => _inputValidation, _logger);
    }

    [DataRow(13.5, 44, 11, 83)] // Small road bike
    [DataRow(14.0, 50, 12, 89)] // Compact road bike
    [DataRow(14.5, 52, 16, 94)] // Standard road bike
    [DataRow(15.0, 53, 28, 102)] // Mountain bike, short chainstay
    [DataRow(15.5, 36, 32, 98)] // Mountain bike, medium chainstay
    [DataRow(16.0, 42, 36, 105)] // Touring - gravel bike
    [TestMethod]
    public void Calculate_ShouldGive_ExpectedResults(double chainStayLengthInch, int teethLargestSprocket, int teethLargestChainring, int expectedChainLink)
    {
        // Arrange
        ChainLengthCalculatorInput input = new()
        {
            ChainStayLengthInch =  chainStayLengthInch,
            TeethLargestChainring = teethLargestChainring,
            TeethLargestSprocket = teethLargestSprocket
        };

        // Act
        OperationResult<ChainLengthCalculatorResult> result = _calculator.Start(input);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.ChainLinks.Should().Be(expectedChainLink);
        result.Content.ChainLengthInch.Should().BeApproximately(expectedChainLink * ChainConst.CHAINLINK_LENGTH_INCH, 0.5);
        result.Content.UsedInputs.Should().BeEquivalentTo(input);
        result.Content.CalculatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [TestMethod]
    public void Calculate_WithNullInput_ShouldThrowEx()
    {
        // Act
        Action act = () => _calculator.Start(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    [DataRow(-1, 50, 28)] // Negative chainstay
    [DataRow(15, 0, 28)]  // Zero chainring
    [DataRow(15, 50, -1)] // Negative sprocket
    public void Calculate_WithInvalidInputs_ShouldThrowEx(double chainStayLength, int chainring, int sprocket)
    {
        // Arrange
        var input = new ChainLengthCalculatorInput
        {
            ChainStayLengthInch = chainStayLength,
            TeethLargestChainring = chainring,
            TeethLargestSprocket = sprocket
        };

        // Act
        Action act = () => _calculator.Start(input);

        // Assert
        act.Should().Throw<CalculatorInputException>();
    }
}
