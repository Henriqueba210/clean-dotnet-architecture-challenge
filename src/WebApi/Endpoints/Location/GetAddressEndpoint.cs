using Ardalis.Result;

using Location.Api.Common.Mapping;
using Location.Application.Locations.Query.GetLocationByPostcode;

using MediatR;

using Microsoft.Extensions.Caching.Hybrid;

using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Location.Api.Endpoints.Location;

public static class GetAddressEndpoint
{
    private static async Task<Result<Domain.Entities.Location>> GetLocationAsync(
        GetLocationByPostcodeQuery query,
        IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(query, cancellationToken);
    }

    public static async Task<IResult> HandleAsync(
        string postCode,
        IMediator mediator,
        HybridCache hybridCache,
        CancellationToken cancellationToken
    )
    {
        Result<Domain.Entities.Location> response =
            await hybridCache.GetOrCreateAsync<GetLocationByPostcodeQuery, Result<Domain.Entities.Location>>(
                postCode,
                new GetLocationByPostcodeQuery(postCode),
                async (query, ct) => await GetLocationAsync(query, mediator, ct),
                new HybridCacheEntryOptions { Expiration = TimeSpan.FromMinutes(5) },
                cancellationToken: cancellationToken);

        return response.ToTypedResult<Domain.Entities.Location, Location>();
    }
}