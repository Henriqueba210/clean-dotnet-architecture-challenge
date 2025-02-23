using Geolocation;

using Location.Domain.Entities;

namespace Location.Application.Common.Calculations;

public static class DistanceCalculator
{
    private static readonly Coordinate HeathrowAirportCoords = new(51.4700223, -0.4542955);

    public static DistanceToHeathrowAirport Calculate(double latitude, double longitude, int decimalPlaces = 2)
    {
        Coordinate destinationCoordinate = new(latitude, longitude);

        double kilometers = GeoCalculator.GetDistance(
            HeathrowAirportCoords,
            destinationCoordinate,
            decimalPlaces,
            DistanceUnit.Kilometers
        );

        double miles = GeoCalculator.GetDistance(
            HeathrowAirportCoords,
            destinationCoordinate,
            decimalPlaces
        );

        return new DistanceToHeathrowAirport(
            kilometers,
            miles
        );
    }
}