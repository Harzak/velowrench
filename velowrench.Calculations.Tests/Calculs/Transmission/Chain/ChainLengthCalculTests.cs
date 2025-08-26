using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Constants;
using velowrench.Calculations.Exceptions;
using velowrench.Calculations.Interfaces;
using velowrench.Calculations.Validation;
using velowrench.Calculations.Validation.Builder;
using velowrench.Calculations.Validation.Results;
using velowrench.Core.Validation;
using velowrench.Utils.Results;

namespace velowrench.Calculations.Tests.Calculs.Transmission.Chain;


[TestClass]
public class ChainLengthCalculTests
{
    private ILogger _logger;
    private ICalculatorInputValidator<ChainLengthCalculatorInput> _inputValidator;
    private ChainLengthCalculator _calculator;

    [TestInitialize]
    public void Initialize()
    {
        _logger = A.Fake<ILogger>();
        _inputValidator = A.Fake<ICalculatorInputValidator<ChainLengthCalculatorInput>>();
        _calculator = new ChainLengthCalculator(_inputValidator, _logger);

        A.CallTo(() => _inputValidator.ValidateWithResults(A<ChainLengthCalculatorInput>._, A<ValidationContext?>._))
            .Returns(ValidationResult.WithSuccess());
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
            ChainStayLengthIn =   chainStayLengthInch,
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
        result.Content.ChainLengthIn.Should().BeApproximately(expectedChainLink * ChainConst.CHAINLINK_LENGTH_INCH, 0.5);
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
}
