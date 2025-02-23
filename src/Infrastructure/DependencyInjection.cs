using System.Diagnostics.CodeAnalysis;

using Infrastructure.Services;
using Infrastructure.Services.Postcodes.Io;

using Location.Application.Abstractions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Refit;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddRefitClient<IPostCodeIoService>()
            .ConfigureHttpClient((sp, httpClient) =>
            {
                PostcodesIoSettings settings = sp.GetRequiredService<IOptions<PostcodesIoSettings>>().Value;

                httpClient.BaseAddress = new Uri(settings.BaseAddress);
            });
        services.AddScoped<ILocationService, LocationService>();

        return services;
    }
}