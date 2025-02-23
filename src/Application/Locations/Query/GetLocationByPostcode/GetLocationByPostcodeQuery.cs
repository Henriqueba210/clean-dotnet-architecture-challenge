using Location.Application.Abstractions.Messaging;

namespace Location.Application.Locations.Query.GetLocationByPostcode;

public sealed record GetLocationByPostcodeQuery(string PostCode) : IQuery<Domain.Entities.Location>;