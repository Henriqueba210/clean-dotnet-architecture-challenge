using Refit;

namespace Infrastructure.Services.Postcodes.Io;

public interface IPostCodeIoService
{
    [Get("/postcodes/{postCode}")]
    Task<PostCodeResponse> GetByPostcodeAsync(string postCode, CancellationToken cancellationToken);
}