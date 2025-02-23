using Ardalis.Result;

using Location.Application.Abstractions;
using Location.Application.Abstractions.Messaging;
using Location.Application.Common.Calculations;

namespace Location.Application.Locations.Query.GetLocationByPostcode;

internal sealed class GetLocationByPostcodeQueryHandler(ILocationService locationService)
    : IQueryHandler<GetLocationByPostcodeQuery, Domain.Entities.Location>
{
    public async Task<Result<Domain.Entities.Location>> Handle(GetLocationByPostcodeQuery request,
        CancellationToken cancellationToken)
    {
        Result<Domain.Entities.Location> location =
            await locationService.GetByPostcodeAsync(request.PostCode, cancellationToken);

        if (!location.IsSuccess)
        {
            return location;
        }

        return Result.Success(location.Value with
        {
            DistanceToHeathrowAirport =
            DistanceCalculator.Calculate(location.Value.Latitude, location.Value.Longitude)
        });
    }
}