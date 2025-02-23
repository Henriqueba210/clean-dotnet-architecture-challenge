namespace WebApi.Integration.Test.Builders;

public static class HybridCacheBuilder
{
    public static Mock<HybridCache> CreateMockCache()
    {
        var cache = new Dictionary<string, object>();
        var mock = new Mock<HybridCache>();

        mock.Setup(x => x.GetOrCreateAsync(
                It.IsAny<string>(),
                It.IsAny<GetLocationByPostcodeQuery>(),
                It.IsAny<Func<GetLocationByPostcodeQuery, CancellationToken, ValueTask<Result<Location.Domain.Entities.Location>>>>(),
                It.IsAny<HybridCacheEntryOptions>(),
                It.IsAny<IEnumerable<string>>(),
                It.IsAny<CancellationToken>()))
            .Returns<string, GetLocationByPostcodeQuery, Func<GetLocationByPostcodeQuery, CancellationToken, ValueTask<Result<Location.Domain.Entities.Location>>>, HybridCacheEntryOptions, object?, CancellationToken>(
                async (key, query, factory, options, state, token) =>
                {
                    if (cache.TryGetValue(key, out object? value))
                    {
                        return (Result<Location.Domain.Entities.Location>)value;
                    }

                    value = await factory(query, token);
                    cache[key] = value;
                    return (Result<Location.Domain.Entities.Location>)value;
                });

        return mock;
    }
}