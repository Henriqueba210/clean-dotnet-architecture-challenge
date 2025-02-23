namespace Location.Api.Endpoints.Location;

[Serializable]
public record Location(
    string Postcode,
    int Quality,
    int? Eastings,
    int? Northings,
    string Country,
    string? NhsHa,
    double Longitude,
    double? Latitude,
    string? Region,
    string? AdminDistrict,
    string? AdminCounty,
    string? AdminWard,
    LocationCodes? Codes,
    DistanceToHeathrowAirport DistanceToHeathrowAirport
);

[Serializable]
public record LocationCodes(
    string? AdminDistrict,
    string? AdminCounty,
    string? AdminWard,
    string? Parish,
    string? ParliamentaryConstituency
);

[Serializable]
public record DistanceToHeathrowAirport(
    double Kilometers,
    double Miles
);