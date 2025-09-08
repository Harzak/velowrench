using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using velowrench.Calculations.Calculators.Transmission.Gear;
using velowrench.Calculations.Enums;
using velowrench.Calculations.Interfaces;
using velowrench.Calculations.Validation;
using velowrench.Calculations.Validation.Results;
using velowrench.Utils.Results;

namespace velowrench.Calculations.Tests.Calculs.Transmission.Gear;

[TestClass]
public class GearRatioCalculTests
{
    private ILogger _logger;
    private ICalculatorInputValidator<GearCalculatorInput> _inputValidator;
    private IUnitStore _unitStore;
    private GearCalculator _calculator;

    [TestInitialize]
    public void Initialize()
    {
        _logger = A.Fake<ILogger>();
        _inputValidator = A.Fake<ICalculatorInputValidator<GearCalculatorInput>>();
        _unitStore = A.Fake<IUnitStore>();
        _calculator = new GearCalculator(_inputValidator, _unitStore, _logger);

        A.CallTo(() => _inputValidator.ValidateWithResults(A<GearCalculatorInput>._, A<ValidationContext?>._))
            .Returns(ValidationResult.WithSuccess());
    }

    #region Gain ratio

    [TestMethod]
    public void GainRatio_Calculation_StandardInput_ShouldGive_ExpectedResults()
    {
        // Arrange
        GearCalculatorInput input = new()
        {
            CalculatorType = EGearCalculatorType.GainRatio,
            RevolutionPerMinute = null,
            TyreOuterDiameterIn = 27,
            CrankLengthMm = 175,
            TeethNumberSmallChainring = 24,
            TeethNumberMediumChainring = 34,
            TeethNumberLargeOrUniqueChainring = 46,
            Precision = 1
        };
        input.WithSprockets([11, 12, 13, 14, 16, 18, 21, 24, 28]);

        // Act
        OperationResult<GearCalculatorResult> result = _calculator.Start(input);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.ValuesSmallChainring.Should().BeEquivalentTo([4.3, 3.9, 3.6, 3.4, 2.9, 2.6, 2.2, 2, 1.7]);
        result.Content.ValuesMediumChainring.Should().BeEquivalentTo([6.1, 5.6, 5.1, 4.8, 4.2, 3.7, 3.2, 2.8, 2.4]);
        result.Content.ValuesLargeOrUniqueChainring.Should().BeEquivalentTo([8.2, 7.5, 6.9, 6.4, 5.6, 5, 4.3, 3.8, 3.2]);
    }

    [TestMethod]
    public void GainRatio_Calculation_LowInput_ShouldGive_ExpectedResults()
    {
        // Arrange
        GearCalculatorInput input = new()
        {
            CalculatorType = EGearCalculatorType.GainRatio,
            RevolutionPerMinute = null,
            TyreOuterDiameterIn = 24,
            CrankLengthMm = 115,
            TeethNumberSmallChainring = 20,
            TeethNumberMediumChainring = 24,
            TeethNumberLargeOrUniqueChainring = 28,
            Precision = 1
        };
        input.WithSprockets([11, 12, 13, 14, 16, 18, 21, 24, 28]);

        // Act
        OperationResult<GearCalculatorResult> result = _calculator.Start(input);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.ValuesSmallChainring.Should().BeEquivalentTo([4.8, 4.4, 4.1, 3.8, 3.3, 2.9, 2.5, 2.2, 1.9]);
        result.Content.ValuesMediumChainring.Should().BeEquivalentTo([5.8, 5.3, 4.9, 4.5, 4, 3.5, 3, 2.7, 2.3]);
        result.Content.ValuesLargeOrUniqueChainring.Should().BeEquivalentTo([6.7, 6.2, 5.7, 5.3, 4.6, 4.1, 3.5, 3.1, 2.7]);
    }


    [TestMethod]
    public void GainRatio_Calculation_HighInput_ShouldGive_ExpectedResults()
    {
        // Arrange
        GearCalculatorInput input = new()
        {
            CalculatorType = EGearCalculatorType.GainRatio,
            RevolutionPerMinute = null,
            TyreOuterDiameterIn = 35.65,
            CrankLengthMm = 190,
            TeethNumberSmallChainring = 26,
            TeethNumberMediumChainring = 32,
            TeethNumberLargeOrUniqueChainring = 52,
            Precision = 1
        };
        input.WithSprockets([10, 12, 14, 16, 18, 21, 24, 28, 32, 36, 42, 50]);

        // Act
        OperationResult<GearCalculatorResult> result = _calculator.Start(input);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.ValuesSmallChainring.Should().BeEquivalentTo([6.2, 5.2, 4.4, 3.9, 3.4, 3, 2.6, 2.2, 1.9, 1.7, 1.5, 1.2]);
        result.Content.ValuesMediumChainring.Should().BeEquivalentTo([7.6, 6.4, 5.4, 4.8, 4.2, 3.6, 3.2, 2.7, 2.4, 2.1, 1.8, 1.5]);
        result.Content.ValuesLargeOrUniqueChainring.Should().BeEquivalentTo([12.4, 10.3, 8.9, 7.7, 6.9, 5.9, 5.2, 4.4, 3.9, 3.4, 3, 2.5]);
    }
    #endregion

    #region Speed

    [TestMethod]
    public void Speed_Calculation_StandardInput_ShouldGive_ExpectedResults()
    {
        // Arrange
        GearCalculatorInput input = new()
        {
            CalculatorType = EGearCalculatorType.Speed,
            RevolutionPerMinute = 90,
            TyreOuterDiameterIn = 27,
            CrankLengthMm = 0,
            TeethNumberSmallChainring = 24,
            TeethNumberMediumChainring = 34,
            TeethNumberLargeOrUniqueChainring = 46,
            Precision = 1
        };
        input.WithSprockets([11, 12, 13, 14, 16, 18, 21, 24, 28]);

        // Act
        OperationResult<GearCalculatorResult> result = _calculator.Start(input);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.ValuesSmallChainring.Should().BeEquivalentTo([25.4, 23.3, 21.5, 19.9, 17.5, 15.5, 13.3, 11.6, 10]);
        result.Content.ValuesMediumChainring.Should().BeEquivalentTo([36, 33, 30.4, 28.3, 24.7, 22, 18.8, 16.5, 14.1]);
        result.Content.ValuesLargeOrUniqueChainring.Should().BeEquivalentTo([48.7, 44.6, 41.2, 38.2, 33.4, 29.7, 25.5, 22.3, 19.1]);
    }

    [TestMethod]
    public void Speed_Calculation_LowInput_ShouldGive_ExpectedResults()
    {
        // Arrange
        GearCalculatorInput input = new()
        {
            CalculatorType = EGearCalculatorType.Speed,
            RevolutionPerMinute = 40,
            TyreOuterDiameterIn = 24,
            CrankLengthMm = 0,
            TeethNumberSmallChainring = 20,
            TeethNumberMediumChainring = 24,
            TeethNumberLargeOrUniqueChainring = 28,
            Precision = 1
        };
        input.WithSprockets([11, 12, 13, 14, 16, 18, 21, 24, 28]);

        // Act
        OperationResult<GearCalculatorResult> result = _calculator.Start(input);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.ValuesSmallChainring.Should().BeEquivalentTo([8.4, 7.7, 7.1, 6.6, 5.7, 5.1, 4.4, 3.8, 3.3]);
        result.Content.ValuesMediumChainring.Should().BeEquivalentTo([10, 9.2, 8.5, 7.9, 6.9, 6.1, 5.3, 4.6, 3.9]);
        result.Content.ValuesLargeOrUniqueChainring.Should().BeEquivalentTo([11.7, 10.7, 9.9, 9.2, 8, 7.2, 6.1, 5.4, 4.6]);
    }

    [TestMethod]
    public void Speed_Calculation_HighInput_ShouldGive_ExpectedResults()
    {
        // Arrange
        GearCalculatorInput input = new()
        {
            CalculatorType = EGearCalculatorType.Speed,
            RevolutionPerMinute = 120,
            TyreOuterDiameterIn = 35.65,
            CrankLengthMm = 0,
            TeethNumberSmallChainring = 26,
            TeethNumberMediumChainring = 32,
            TeethNumberLargeOrUniqueChainring = 52,
            Precision = 1
        };
        input.WithSprockets([10, 12, 14, 16, 18, 21, 24, 28, 32, 36, 42, 50]);

        // Act
        OperationResult<GearCalculatorResult> result = _calculator.Start(input);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.ValuesSmallChainring.Should().BeEquivalentTo([53.3, 44.4, 38, 33.3, 29.6, 25.4, 22.2, 19, 16.6, 14.8, 12.7, 10.7]);
        result.Content.ValuesMediumChainring.Should().BeEquivalentTo([65.5, 54.6, 46.8, 41, 36.4, 31.2, 27.3, 23.4, 20.5, 18.2, 15.6, 13.1]);
        result.Content.ValuesLargeOrUniqueChainring.Should().BeEquivalentTo([106.5, 88.8, 76.1, 66.6, 59.2, 50.7, 44.4, 38, 33.3, 29.6, 25.4, 21.3]);
    }
    #endregion

    #region Gear inches

    [TestMethod]
    public void GearInches_Calculation_StandardInput_ShouldGive_ExpectedResults()
    {
        // Arrange
        GearCalculatorInput input = new()
        {
            CalculatorType = EGearCalculatorType.GearInches,
            RevolutionPerMinute = null,
            TyreOuterDiameterIn = 27,
            CrankLengthMm = 175,
            TeethNumberSmallChainring = 24,
            TeethNumberMediumChainring = 34,
            TeethNumberLargeOrUniqueChainring = 46,
            Precision = 1
        };
        input.WithSprockets([11, 12, 13, 14, 16, 18, 21, 24, 28]);

        // Act
        OperationResult<GearCalculatorResult> result = _calculator.Start(input);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.ValuesSmallChainring.Should().BeEquivalentTo([58.9, 54, 49.8, 46.3, 40.5, 36, 30.9, 27, 23.1]);
        result.Content.ValuesMediumChainring.Should().BeEquivalentTo([83.5, 76.5, 70.6, 65.6, 57.4, 51, 43.7, 38.3, 32.8]);
        result.Content.ValuesLargeOrUniqueChainring.Should().BeEquivalentTo([112.9, 103.5, 95.5, 88.7, 77.6, 69, 59.1, 51.8, 44.4]);
    }

    [TestMethod]
    public void GearInches_Calculation_LowInput_ShouldGive_ExpectedResults()
    {
        // Arrange
        GearCalculatorInput input = new()
        {
            CalculatorType = EGearCalculatorType.GearInches,
            RevolutionPerMinute = null,
            TyreOuterDiameterIn = 24,
            CrankLengthMm = 115,
            TeethNumberSmallChainring = 20,
            TeethNumberMediumChainring = 24,
            TeethNumberLargeOrUniqueChainring = 28,
            Precision = 1
        };
        input.WithSprockets([11, 12, 13, 14, 16, 18, 21, 24, 28]);

        // Act
        OperationResult<GearCalculatorResult> result = _calculator.Start(input);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.ValuesSmallChainring.Should().BeEquivalentTo([43.6, 40, 36.9, 34.3, 30, 26.7, 22.9, 20, 17.1]);
        result.Content.ValuesMediumChainring.Should().BeEquivalentTo([52.4, 48, 44.3, 41.1, 36, 32, 27.4, 24, 20.6]);
        result.Content.ValuesLargeOrUniqueChainring.Should().BeEquivalentTo([61.1, 56, 51.7, 48, 42, 37.3, 32, 28, 24]);
    }

    [TestMethod]
    public void GearInches_Calculation_HighInput_ShouldGive_ExpectedResults()
    {
        // Arrange
        GearCalculatorInput input = new()
        {
            CalculatorType = EGearCalculatorType.GearInches,
            RevolutionPerMinute = null,
            TyreOuterDiameterIn = 35.65,
            CrankLengthMm = 190,
            TeethNumberSmallChainring = 26,
            TeethNumberMediumChainring = 32,
            TeethNumberLargeOrUniqueChainring = 52,
            Precision = 1
        };
        input.WithSprockets([10, 12, 14, 16, 18, 21, 24, 28, 32, 36, 42, 50]);

        // Act
        OperationResult<GearCalculatorResult> result = _calculator.Start(input);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.ValuesSmallChainring.Should().BeEquivalentTo([92.7, 77.2, 66.2, 57.9, 51.5, 44.1, 38.6, 33.1, 29, 25.7, 22.1, 18.5]);
        result.Content.ValuesMediumChainring.Should().BeEquivalentTo([114.1, 95.1, 81.5, 71.3, 63.4, 54.3, 47.5, 40.7, 35.6, 31.7, 27.2, 22.8]);
        result.Content.ValuesLargeOrUniqueChainring.Should().BeEquivalentTo([185.4, 154.5, 132.4, 115.9, 103, 88.3, 77.2, 66.2, 57.9, 51.5, 44.1, 37.1]);
    }
    #endregion

    #region Development

    [TestMethod]
    public void Development_Calculation_StandardInput_ShouldGive_ExpectedResults()
    {
        // Arrange
        GearCalculatorInput input = new()
        {
            CalculatorType = EGearCalculatorType.Development,
            RevolutionPerMinute = null,
            TyreOuterDiameterIn = 27,
            CrankLengthMm = 175,
            TeethNumberSmallChainring = 24,
            TeethNumberMediumChainring = 34,
            TeethNumberLargeOrUniqueChainring = 46,
            Precision = 1
        };
        input.WithSprockets([11, 12, 13, 14, 16, 18, 21, 24, 28]);

        // Act
        OperationResult<GearCalculatorResult> result = _calculator.Start(input);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.ValuesSmallChainring.Should().BeEquivalentTo([4.7, 4.3, 4, 3.7, 3.2, 2.9, 2.5, 2.2, 1.8]);
        result.Content.ValuesMediumChainring.Should().BeEquivalentTo([6.7, 6.1, 5.6, 5.2, 4.6, 4.1, 3.5, 3.1, 2.6]);
        result.Content.ValuesLargeOrUniqueChainring.Should().BeEquivalentTo([9, 8.3, 7.6, 7.1, 6.2, 5.5, 4.7, 4.1, 3.5]);
    }

    [TestMethod]
    public void Development_Calculation_LowInput_ShouldGive_ExpectedResults()
    {
        // Arrange
        GearCalculatorInput input = new()
        {
            CalculatorType = EGearCalculatorType.Development,
            RevolutionPerMinute = null,
            TyreOuterDiameterIn = 24,
            CrankLengthMm = 115,
            TeethNumberSmallChainring = 20,
            TeethNumberMediumChainring = 24,
            TeethNumberLargeOrUniqueChainring = 28,
            Precision = 1
        };
        input.WithSprockets([11, 12, 13, 14, 16, 18, 21, 24, 28]);

        // Act
        OperationResult<GearCalculatorResult> result = _calculator.Start(input);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.ValuesSmallChainring.Should().BeEquivalentTo([3.5, 3.2, 2.9, 2.7, 2.4, 2.1, 1.8, 1.6, 1.4]);
        result.Content.ValuesMediumChainring.Should().BeEquivalentTo([4.2, 3.8, 3.5, 3.3, 2.9, 2.6, 2.2, 1.9, 1.6]);
        result.Content.ValuesLargeOrUniqueChainring.Should().BeEquivalentTo([4.9, 4.5, 4.1, 3.8, 3.4, 3, 2.6, 2.2, 1.9]);
    }

    [TestMethod]
    public void Development_Calculation_HighInput_ShouldGive_ExpectedResults()
    {
        // Arrange
        GearCalculatorInput input = new()
        {
            CalculatorType = EGearCalculatorType.Development,
            RevolutionPerMinute = null,
            TyreOuterDiameterIn = 35.65,
            CrankLengthMm = 190,
            TeethNumberSmallChainring = 26,
            TeethNumberMediumChainring = 32,
            TeethNumberLargeOrUniqueChainring = 52,
            Precision = 1
        };
        input.WithSprockets([10, 12, 14, 16, 18, 21, 24, 28, 32, 36, 42, 50]);

        // Act
        OperationResult<GearCalculatorResult> result = _calculator.Start(input);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.ValuesSmallChainring.Should().BeEquivalentTo([7.4, 6.2, 5.3, 4.6, 4.1, 3.5, 3.1, 2.6, 2.3, 2.1, 1.8, 1.5]);
        result.Content.ValuesMediumChainring.Should().BeEquivalentTo([9.1, 7.6, 6.5, 5.7, 5.1, 4.3, 3.8, 3.3, 2.8, 2.5, 2.2, 1.8]);
        result.Content.ValuesLargeOrUniqueChainring.Should().BeEquivalentTo([14.8, 12.3, 10.6, 9.2, 8.2, 7, 6.2, 5.3, 4.6, 4.1, 3.5, 3]);
    }
    #endregion
}