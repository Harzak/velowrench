using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculators.Transmission.Gear;
using velowrench.Calculations.Enums;
using velowrench.Utils.Results;

namespace velowrench.Calculations.Tests.Calculs.Transmission.Gear.Gain;

[TestClass]
public class GainRatioCalculTests
{
    private ILogger _logger;
    private GearCalculator _calculator;

    [TestInitialize]
    public void Initialize()
    {
        _logger = A.Fake<ILogger>();
        _calculator = new GearCalculator(() => new GearCalculatorInputValidator(), _logger);
    }

    [TestMethod]
    public void Calculate_ShouldGive_ExpectedResults()
    {
        // Arrange
        GearCalculatorInput input = new()
        {
            CalculatorType = EGearCalculatorType.GainRatio,
            RevolutionPerMinute = null,
            WheelDiameterInInch = 27,
            CrankLengthInMilimeter = 170,
            NumberOfTeethBySprocket = [11, 12, 13, 14, 16, 18, 21, 24, 28],
            TeethNumberSmallChainring = null,
            TeethNumberMediumChainring = null,
            TeethNumberLargeOrUniqueChainring = 45,
            Precision = 1
        };

        // Act
        OperationResult<GearCalculatorResult> result = _calculator.Start(input);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.ValuesLargeOrUniqueChainring.Should().BeEquivalentTo([8.3, 7.6, 7, 6.5, 5.7, 5, 4.3, 3.8, 3.2,]);
    }
}