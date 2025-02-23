using Microsoft.AspNetCore.Mvc;

namespace Location.Api.Endpoints.Location;

public static class LocationEndpoints
{
    public static void MapLocationEndpoints(this WebApplication app)
    {
        RouteGroupBuilder? group = app.MapGroup("/api/location");

        group.MapGet("/address/{postcode}", GetAddressEndpoint.HandleAsync)
            .Produces<Location>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithName("GetLocationByPostcode");
    }
}