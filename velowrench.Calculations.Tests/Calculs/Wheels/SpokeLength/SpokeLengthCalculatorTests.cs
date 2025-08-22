using FakeItEasy;
using Microsoft.Extensions.Logging;
using velowrench.Calculations.Calculators.Wheels.SpokeLength;
using velowrench.Calculations.Interfaces;
using velowrench.Utils.Results;

namespace velowrench.Calculations.Tests.Calculs.Wheels.SpokeLength;

[TestClass]
public class SpokeLengthCalculatorTests
{
    private ILogger _logger;
    private ICalculatorInputValidation<SpokeLengthCalculatorInput> _inputValidation;
    private SpokeLengthCalculator _calculator;


    [TestInitialize]
    public void Initialize()
    {
        _logger = A.Fake<ILogger>();
        _inputValidation = new SpokeLengthCalculatorInputValidation();
        _calculator = new SpokeLengthCalculator(() => _inputValidation, _logger);
    }

    [TestMethod] /// Minimum (small 20″ front wheel, simple hub)
    public void Calculate_WithMininumInputValue_ShouldGive_ExpectedResults()
    {
        // Arrange
        SpokeLengthCalculatorInput input = new()
        {
            HubCenterToFlangeDistanceGearSideMm = 32,
            HubCenterToFlangeDistanceNonGearSideMm = 32,
            HubFlangeDiameterGearSideMm = 45,
            HubFlangeDiameterNonGearSideMm = 45,
            RimInternalDiameterMm = 395,
            SpokeCount = 36,
            SpokeLacingPattern = 2,
            Precision = 0
        };

        // Act
        OperationResult<SpokeLengthCalculatorResult> result = _calculator.Start(input);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Content);
        Assert.AreEqual(182, result.Content.SpokeLengthNonGearSideMm);
        Assert.AreEqual(182, result.Content.SpokeLengthGearSideMm);
    }

    [TestMethod] /// 700C/29″ rear road/gravel, 130–135 mm hub
    public void Calculate_WithMostCommonInputValue_ShouldGive_ExpectedResults()
    {
        // Arrange
        SpokeLengthCalculatorInput input = new()
        {
            HubCenterToFlangeDistanceGearSideMm = 36,
            HubCenterToFlangeDistanceNonGearSideMm = 17,
            HubFlangeDiameterGearSideMm = 45,
            HubFlangeDiameterNonGearSideMm = 45,
            RimInternalDiameterMm = 600,
            SpokeCount = 32,
            SpokeLacingPattern = 3,
            Precision = 0
        };

        // Act
        OperationResult<SpokeLengthCalculatorResult> result = _calculator.Start(input);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Content);
        Assert.AreEqual(291, result.Content.SpokeLengthNonGearSideMm);
        Assert.AreEqual(293, result.Content.SpokeLengthGearSideMm);
    }

    [TestMethod] /// Fat bike rear, 197 mm hub, big flanges
    public void Calculate_WithMaximumInputValue_ShouldGive_ExpectedResults()
    {
        // Arrange
        SpokeLengthCalculatorInput input = new()
        {
            HubCenterToFlangeDistanceGearSideMm = 60,
            HubCenterToFlangeDistanceNonGearSideMm = 60,
            HubFlangeDiameterGearSideMm = 70,
            HubFlangeDiameterNonGearSideMm = 70,
            RimInternalDiameterMm = 560,
            SpokeCount = 32,
            SpokeLacingPattern = 3,
            Precision = 0
        };

        // Act
        OperationResult<SpokeLengthCalculatorResult> result = _calculator.Start(input);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Content);
        Assert.AreEqual(274, result.Content.SpokeLengthNonGearSideMm);
        Assert.AreEqual(274, result.Content.SpokeLengthGearSideMm);
    }

}

