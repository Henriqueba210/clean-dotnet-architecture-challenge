namespace WebApi.Integration.Test.Builders;

public static class PostCodeResponseBuilder
{
    private static readonly PostCodeResponse DefaultResponse = new(
        200,
        new PostCodeData(
            "SW1A 1AA",
            1,
            529090,
            179645,
            "England",
            "London",
            -0.141588,
            51.501009,
            "London",
            "Westminster",
            "Greater London",
            "St James's",
            new LocationCodes(
                "E09000033",
                "E99999999",
                "E05000644",
                "E43000218",
                "E14000639")));

    public static Mock<IPostCodeIoService> CreateMockPostCodeService()
    {
        var mock = new Mock<IPostCodeIoService>();

        mock.Setup(x => x.GetByPostcodeAsync("SW1A 1AA", It.IsAny<CancellationToken>()))
            .ReturnsAsync(DefaultResponse);

        var notFoundException = CreateNotFoundException();
        mock.Setup(x => x.GetByPostcodeAsync(It.Is<string>(s => s != "SW1A 1AA"), It.IsAny<CancellationToken>()))
            .ThrowsAsync(notFoundException);

        return mock;
    }

    private static ApiException CreateNotFoundException()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            RequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://postcodes.io"),
            Content = new StringContent("Resource not found")
        };

        return ApiException.Create(
            "Not Found",
            new HttpRequestMessage(HttpMethod.Get, "https://postcodes.io"),
            HttpMethod.Get,
            response,
            new RefitSettings(),
            null).GetAwaiter().GetResult();
    }
}