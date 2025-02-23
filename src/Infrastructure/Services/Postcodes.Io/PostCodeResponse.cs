namespace Infrastructure.Services.Postcodes.Io;

public record PostCodeResponse(
    int Status,
    PostCodeData? Result
);

public record PostCodeData(
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
    LocationCodes? Codes
);

public record LocationCodes(
    string? AdminDistrict,
    string? AdminCounty,
    string? AdminWard,
    string? Parish,
    string? ParliamentaryConstituency
);