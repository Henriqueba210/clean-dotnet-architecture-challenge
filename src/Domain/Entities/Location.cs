namespace Location.Domain.Entities;

public record Location(
    string Postcode,
    int Quality,
    int? Eastings,
    int? Northings,
    string Country,
    string? NhsHa,
    double Longitude,
    double Latitude,
    string? Region,
    string? AdminDistrict,
    string? AdminCounty,
    string? AdminWard,
    LocationCodes? Codes,
    DistanceToHeathrowAirport? DistanceToHeathrowAirport
);

public record LocationCodes(
    string? AdminDistrict,
    string? AdminCounty,
    string? AdminWard,
    string? Parish,
    string? ParliamentaryConstituency
);

public record DistanceToHeathrowAirport(
    double Kilometers,
    double Miles
);