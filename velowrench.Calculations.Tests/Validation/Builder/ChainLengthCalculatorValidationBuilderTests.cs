using FluentAssertions;
using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Interfaces;
using velowrench.Calculations.Validation;
using velowrench.Calculations.Validation.Builder;
using velowrench.Calculations.Validation.Results;

namespace velowrench.Calculations.Tests.Validation.Builder;

[TestClass]
public class ChainLengthCalculatorValidationBuilderTests
{
    private ICalculatorInputValidator<ChainLengthCalculatorInput> _validator;

    [TestInitialize]
    public void Initialize()
    {
        ChainLengthcalculatorValidationBuilder builder = new();
        _validator = new CalculatorInputValidator<ChainLengthCalculatorInput>(builder);
    }

    [TestMethod]
    public void ChainLengthValidation_WithValidInput_ShouldPass()
    {
        // Arrange
        var input = new ChainLengthCalculatorInput
        {
            ChainStayLengthIn = 16.5,
            TeethLargestChainring = 50,
            TeethLargestSprocket = 28
        };

        // Act
        ValidationResult result = _validator.ValidateWithResults(input);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [TestMethod]
    public void ChainLengthValidation_WithInvalidChainStay_ShouldFail()
    {
        // Arrange
        var input = new ChainLengthCalculatorInput
        {
            ChainStayLengthIn = 0.5,
            TeethLargestChainring = 50,
            TeethLargestSprocket = 28
        };

        // Act
        ValidationResult result = _validator.ValidateWithResults(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(input.ChainStayLengthIn));
        result.Errors.First(x => x.PropertyName == nameof(input.ChainStayLengthIn)).Message.Should().Contain("greater than 1");
    }

    [TestMethod]
    public void ChainLengthValidation_WithInvalidChainring_ShouldFail()
    {
        // Arrange
        var input = new ChainLengthCalculatorInput
        {
            ChainStayLengthIn = 16.5,
            TeethLargestChainring = 5,
            TeethLargestSprocket = 28
        };

        // Act
        ValidationResult result = _validator.ValidateWithResults(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(input.TeethLargestChainring));
        result.Errors.First(x => x.PropertyName == nameof(input.TeethLargestChainring)).Message.Should().Contain("between 10 and 120");
    }

    [TestMethod]
    public void ChainLengthValidation_WithInvalidSprocket_ShouldFail()
    {
        // Arrange
        var input = new ChainLengthCalculatorInput
        {
            ChainStayLengthIn = 16.5,
            TeethLargestChainring = 50,
            TeethLargestSprocket = 60
        };

        // Act
        ValidationResult result = _validator.ValidateWithResults(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(input.TeethLargestSprocket));
        result.Errors.First(x => x.PropertyName == nameof(input.TeethLargestSprocket)).Message.Should().Contain("between 9 and 54");
    }

    [TestMethod]
    public void ChainLengthValidation_WithMultipleErrors_ShouldReturnAllErrors()
    {
        // Arrange
        var input = new ChainLengthCalculatorInput
        {
            ChainStayLengthIn = 0.5,
            TeethLargestChainring = 5,
            TeethLargestSprocket = 60
        };

        // Act
        ValidationResult result = _validator.ValidateWithResults(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(3);
        result.Errors.Should().Contain(x => x.PropertyName == nameof(input.ChainStayLengthIn));
        result.Errors.Should().Contain(x => x.PropertyName == nameof(input.TeethLargestChainring));
        result.Errors.Should().Contain(x => x.PropertyName == nameof(input.TeethLargestSprocket));
    }


    [TestMethod]
    public void ChainLengthValidation_ValidateProperty_ShouldOnlyValidateSpecificProperty()
    {
        // Arrange
        var input = new ChainLengthCalculatorInput
        {
            ChainStayLengthIn = 0.5,
            TeethLargestChainring = 5,
            TeethLargestSprocket = 28
        };

        // Act
        ValidationResult result = _validator.ValidateProperty(input, nameof(input.TeethLargestSprocket));

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
