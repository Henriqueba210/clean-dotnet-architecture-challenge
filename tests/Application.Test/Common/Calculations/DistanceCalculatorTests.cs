namespace Application.Test.Common.Calculations;

public class DistanceCalculatorTests
{
    private const double HeathrowLatitude = 51.4700;
    private const double HeathrowLongitude = -0.4543;
    private const double ValidLatitude = 51.5072178;
    private const double ValidLongitude = -0.1275862;
    private const double InvalidLatitude = 200.0;
    private const double InvalidLongitude = -200.0;

    [Fact]
    public void Calculate_WithValidCoordinates_ShouldReturnCorrectDistance()
    {
        // Act
        DistanceToHeathrowAirport distance = DistanceCalculator.Calculate(ValidLatitude, ValidLongitude);

        // Assert
        distance.Kilometers.ShouldBeInRange(23.0, 24.0);
        distance.Miles.ShouldBeInRange(14.0, 15.0);
    }

    [Fact]
    public void Calculate_WithInvalidLatitude_ShouldThrowArgumentException()
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() =>
            DistanceCalculator.Calculate(InvalidLatitude, ValidLongitude));
    }

    [Fact]
    public void Calculate_WithInvalidLongitude_ShouldThrowArgumentException()
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() =>
            DistanceCalculator.Calculate(ValidLatitude, InvalidLongitude));
    }

    [Fact]
    public void Calculate_WithSameCoordinates_ShouldReturnZeroDistance()
    {
        // Act
        DistanceToHeathrowAirport distance = DistanceCalculator.Calculate(HeathrowLatitude, HeathrowLongitude);

        // Assert
        distance.Kilometers.ShouldBe(0);
        distance.Miles.ShouldBe(0);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(4)]
    public void Calculate_WithDifferentDecimalPlaces_ShouldRoundCorrectly(int decimalPlaces)
    {
        // Act
        DistanceToHeathrowAirport distance = DistanceCalculator.Calculate(ValidLatitude, ValidLongitude, decimalPlaces);

        // Assert
        string kmString = distance.Kilometers.ToString("F" + decimalPlaces, CultureInfo.InvariantCulture);
        string miString = distance.Miles.ToString("F" + decimalPlaces, CultureInfo.InvariantCulture);

        kmString.Split('.')[1].Length.ShouldBe(decimalPlaces);
        miString.Split('.')[1].Length.ShouldBe(decimalPlaces);
    }

    [Fact]
    public void Calculate_WithZeroDecimalPlaces_ShouldReturnWholeNumbers()
    {
        // Act
        DistanceToHeathrowAirport distance = DistanceCalculator.Calculate(ValidLatitude, ValidLongitude, 0);

        // Assert
        distance.Kilometers.ShouldBe(Math.Floor(distance.Kilometers));
        distance.Miles.ShouldBe(Math.Floor(distance.Miles));
    }
}