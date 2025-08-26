using FluentAssertions;
using UnitsNet;
using UnitsNet.Units;
using velowrench.Core.Units;

namespace velowrench.Core.Tests.Units;

[TestClass]
public class ConvertibleDoubleTests
{
    [TestMethod]
    public void ConvertibleDouble_DefaultConstructor_ShouldInitializeWithZeroValueAndDefaultUnit()
    {
        // Arrange & Act
        var convertible = new ConvertibleDouble<LengthUnit>(LengthUnit.Meter);

        // Assert
        convertible.Value.Should().Be(0);
        convertible.Unit.Should().Be(Length.BaseUnit);
        convertible.IsEmpty.Should().BeTrue();
    }

    [TestMethod]
    public void ConvertibleDouble_ValueAndUnitConstructor_ShouldInitializeWithGivenValueAndUnit()
    {
        // Arrange
        double expectedValue = 10.5;
        LengthUnit expectedUnit = LengthUnit.Meter;

        // Act
        var convertible = new ConvertibleDouble<LengthUnit>(expectedValue, expectedUnit);

        // Assert
        convertible.Value.Should().Be(expectedValue);
        convertible.Unit.Should().Be(expectedUnit);
        convertible.IsEmpty.Should().BeFalse();
    }

    [TestMethod]
    public void ConvertibleDouble_GetValueIn_ShouldConvertValueToSpecifiedUnit()
    {
        // Arrange
        var convertible = new ConvertibleDouble<LengthUnit>(100, LengthUnit.Centimeter);
        double expectedValueInMeters = 1.0;

        // Act
        double actualValueInMeters = convertible.GetValueIn(LengthUnit.Meter, precision: 2);

        // Assert
        actualValueInMeters.Should().Be(expectedValueInMeters);
    }

    [TestMethod]
    public void ConvertibleDouble_ChangingUnit_ShouldConvertValueToNewUnit()
    {
        // Arrange
        var convertible = new ConvertibleDouble<LengthUnit>(100, LengthUnit.Centimeter);
        double expectedValueInMeters = 1.0;

        // Act
        convertible.Unit = LengthUnit.Meter;

        // Assert
        convertible.Value.Should().Be(expectedValueInMeters);
        convertible.Unit.Should().Be(LengthUnit.Meter);
    }
}
