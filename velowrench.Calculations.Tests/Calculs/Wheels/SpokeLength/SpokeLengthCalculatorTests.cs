using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Calculators.Wheels.SpokeLength;
using velowrench.Calculations.Interfaces;
using velowrench.Calculations.Units;
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
            HubCenterToFlangeDistanceGearSide =  new ConvertibleDouble<UnitsNet.Units.LengthUnit>(32, UnitsNet.Units.LengthUnit.Millimeter),
            HubCenterToFlangeDistanceNonGearSide =  new ConvertibleDouble<UnitsNet.Units.LengthUnit>(32, UnitsNet.Units.LengthUnit.Millimeter),
            HubFlangeDiameterGearSide =  new ConvertibleDouble<UnitsNet.Units.LengthUnit>(45, UnitsNet.Units.LengthUnit.Millimeter),
            HubFlangeDiameterNonGearSide =  new ConvertibleDouble<UnitsNet.Units.LengthUnit>(45, UnitsNet.Units.LengthUnit.Millimeter),
            RimInternalDiameter =  new ConvertibleDouble<UnitsNet.Units.LengthUnit>(395, UnitsNet.Units.LengthUnit.Millimeter),
            SpokeCount = 36,
            SpokeLacingPattern = 2
        };

        // Act
        OperationResult<SpokeLengthCalculatorResult> result = _calculator.Start(input);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Content);
        Assert.AreEqual(182, result.Content.SpokeLengthNonGearSide.GetValueIn(UnitsNet.Units.LengthUnit.Millimeter));
        Assert.AreEqual(182, result.Content.SpokeLengthGearSide.GetValueIn(UnitsNet.Units.LengthUnit.Millimeter));
    }

    [TestMethod] /// 700C/29″ rear road/gravel, 130–135 mm hub
    public void Calculate_WithMostCommonInputValue_ShouldGive_ExpectedResults()
    {
        // Arrange
        SpokeLengthCalculatorInput input = new()
        {
            HubCenterToFlangeDistanceGearSide =  new ConvertibleDouble<UnitsNet.Units.LengthUnit>(36, UnitsNet.Units.LengthUnit.Millimeter),
            HubCenterToFlangeDistanceNonGearSide =  new ConvertibleDouble<UnitsNet.Units.LengthUnit>(17, UnitsNet.Units.LengthUnit.Millimeter),
            HubFlangeDiameterGearSide =  new ConvertibleDouble<UnitsNet.Units.LengthUnit>(45, UnitsNet.Units.LengthUnit.Millimeter),
            HubFlangeDiameterNonGearSide =  new ConvertibleDouble<UnitsNet.Units.LengthUnit>(45, UnitsNet.Units.LengthUnit.Millimeter),
            RimInternalDiameter =  new ConvertibleDouble<UnitsNet.Units.LengthUnit>(600, UnitsNet.Units.LengthUnit.Millimeter),
            SpokeCount = 32,
            SpokeLacingPattern = 3
        };

        // Act
        OperationResult<SpokeLengthCalculatorResult> result = _calculator.Start(input);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Content);
        Assert.AreEqual(291, result.Content.SpokeLengthNonGearSide.GetValueIn(UnitsNet.Units.LengthUnit.Millimeter));
        Assert.AreEqual(293,result.Content.SpokeLengthGearSide.GetValueIn(UnitsNet.Units.LengthUnit.Millimeter));
    }

    [TestMethod] /// Fat bike rear, 197 mm hub, big flanges
    public void Calculate_WithMaximumInputValue_ShouldGive_ExpectedResults()
    {
        // Arrange
        SpokeLengthCalculatorInput input = new()
        {
            HubCenterToFlangeDistanceGearSide =  new ConvertibleDouble<UnitsNet.Units.LengthUnit>(60, UnitsNet.Units.LengthUnit.Millimeter),
            HubCenterToFlangeDistanceNonGearSide =  new ConvertibleDouble<UnitsNet.Units.LengthUnit>(60, UnitsNet.Units.LengthUnit.Millimeter),
            HubFlangeDiameterGearSide =  new ConvertibleDouble<UnitsNet.Units.LengthUnit>(70, UnitsNet.Units.LengthUnit.Millimeter),
            HubFlangeDiameterNonGearSide =  new ConvertibleDouble<UnitsNet.Units.LengthUnit>(70, UnitsNet.Units.LengthUnit.Millimeter),
            RimInternalDiameter =  new ConvertibleDouble<UnitsNet.Units.LengthUnit>(560, UnitsNet.Units.LengthUnit.Millimeter),
            SpokeCount = 32,
            SpokeLacingPattern = 3
        };

        // Act
        OperationResult<SpokeLengthCalculatorResult> result = _calculator.Start(input);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Content);
        Assert.AreEqual(274, result.Content.SpokeLengthNonGearSide.GetValueIn(UnitsNet.Units.LengthUnit.Millimeter));
        Assert.AreEqual(274, result.Content.SpokeLengthGearSide.GetValueIn(UnitsNet.Units.LengthUnit.Millimeter));
    }

}

