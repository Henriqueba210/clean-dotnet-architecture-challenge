using Ardalis.Result;

namespace Location.Application.Abstractions;

public interface ILocationService
{
    Task<Result<Domain.Entities.Location>> GetByPostcodeAsync(string postCode, CancellationToken cancellationToken);
}