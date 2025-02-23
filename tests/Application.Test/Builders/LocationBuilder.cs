namespace Application.Test.Builders;

public class LocationBuilder
{
    private const string DefaultPostcode = "SW1A 1AA";
    private const int Quality = 1;
    private const int DefaultEastings = 529090;
    private const int DefaultNorthings = 179645;
    private const string DefaultCountry = "England";
    private const string DefaultNhsHa = "London";
    private const double DefaultLongitude = -0.1275862;
    private const double DefaultLatitude = 51.5072178;
    private const string DefaultRegion = "London";
    private const string DefaultAdminDistrict = "Westminster";
    private const string DefaultAdminCounty = "Greater London";
    private const string DefaultAdminWard = "St James's";
    private const string DefaultAdminDistrictCode = "E09000033";
    private const string DefaultAdminWardCode = "E05000644";
    private const string DefaultParish = "Westminster";
    private const string DefaultConstituency = "Cities of London and Westminster";

    private readonly LocationCodes _codes = new(
        DefaultAdminDistrictCode,
        null,
        DefaultAdminWardCode,
        DefaultParish,
        DefaultConstituency
    );

    private DistanceToHeathrowAirport? _distanceToHeathrowAirport;
    private double _latitude = DefaultLatitude;
    private double _longitude = DefaultLongitude;

    private string _postcode = DefaultPostcode;

    public Location.Domain.Entities.Location Build()
    {
        return new Location.Domain.Entities.Location(
            _postcode,
            Quality,
            DefaultEastings,
            DefaultNorthings,
            DefaultCountry,
            DefaultNhsHa,
            _longitude,
            _latitude,
            DefaultRegion,
            DefaultAdminDistrict,
            DefaultAdminCounty,
            DefaultAdminWard,
            _codes,
            _distanceToHeathrowAirport
        );
    }

    public LocationBuilder WithPostcode(string postcode)
    {
        _postcode = postcode;
        return this;
    }

    public LocationBuilder WithCoordinates(double latitude, double longitude)
    {
        _latitude = latitude;
        _longitude = longitude;
        return this;
    }

    public LocationBuilder WithDistance(double kilometers, double miles)
    {
        _distanceToHeathrowAirport = new DistanceToHeathrowAirport(kilometers, miles);
        return this;
    }
}