using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculs.Transmission.GearRatio;
using velowrench.Calculations.Calculs.Transmission.GearRatio.Gain;
using velowrench.Utils.Results;

namespace velowrench.Calculations.Tests.Calculs.Transmission.GearRatio.Gain;

[TestClass]
public class GainRatioCalculTests
{
    private ILogger _logger;
    private GainRatioCalcul _calcul;

    [TestInitialize]
    public void Initialize()
    {
        _logger = A.Fake<ILogger>();
        _calcul = new GainRatioCalcul(_logger);
    }

    [TestMethod]
    public void Calculate_ShouldGive_ExpectedResults()
    {
        // Arrange
        GainRatioCalculInput input = new()
        {
            WheelDiameterInInch = 27,
            CrankLengthInInch = 6.7,
            NumberOfTeethBySprocket = [11, 12, 13, 14, 16, 18, 21, 24, 28],
            TeethNumberSmallChainring = 34,
            TeethNumberMediumChainring = 45,
            TeethNumberLargeOrUniqueChainring = 53,
            Precision = 3
        };

        // Act
        OperationResult<GearRatioCalculResult> result = _calcul.Start(input);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.RatiosSmallChainring.Should().BeEquivalentTo([6.2, 5.7, 5.3, 4.9, 4.3, 3.8, 3.3, 2.9, 2.5]);
        result.Content.RatiosMediumChainring.Should().BeEquivalentTo([8.3, 7.6, 7, 6.5, 5.7, 5, 4.3, 3.8, 3.2]);
        result.Content.RatiosLargeOrUniqueChainring.Should().BeEquivalentTo([9.7, 8.9, 8.2, 7.6, 6.7, 5.9, 5.1, 4.5, 3.8]);
    }
}
