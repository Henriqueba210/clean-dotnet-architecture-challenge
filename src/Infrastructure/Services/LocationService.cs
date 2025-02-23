using System.Net;

using Ardalis.Result;

using Infrastructure.Services.Postcodes.Io;

using Location.Application.Abstractions;

using Mapster;

using Microsoft.Extensions.Logging;

using Refit;

namespace Infrastructure.Services;

internal sealed class LocationService(IPostCodeIoService postCodeIoService, ILogger<LocationService> logger)
    : ILocationService
{
    public async Task<Result<Location.Domain.Entities.Location>> GetByPostcodeAsync(string postCode,
        CancellationToken cancellationToken)
    {
        try
        {
            PostCodeResponse postCodeData = await postCodeIoService.GetByPostcodeAsync(postCode, cancellationToken);

            return postCodeData.Result.Adapt<Location.Domain.Entities.Location>();
        }
        catch (ApiException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound)
            {
                logger.LogWarning("Postcode {PostCode} not found", postCode);
                return Result.NotFound($"Postcode: {postCode} not found");
            }

            logger.LogError(e, "Critical error in get location by postcode {PostCode}", postCode);
            return Result.CriticalError("Unable to process postcode lookup. Please try again later.");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Critical error in get location by postcode {PostCode}", postCode);
            return Result.CriticalError("Unable to process postcode lookup. Please try again later.");
        }
    }
}