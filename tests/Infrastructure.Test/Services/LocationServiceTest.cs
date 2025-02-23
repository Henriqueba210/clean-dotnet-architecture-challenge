namespace Infrastructure.Test.Services;

public class LocationServiceTests
{
    [Theory]
    [AutoData]
    public async Task GetByPostcodeAsync_WhenSuccessful_ReturnsLocation(
        PostCodeResponse response,
        string postCode)
    {
        // Arrange
        Mock<IPostCodeIoService> postCodeServiceMock = new();
        Mock<ILogger<LocationService>> loggerMock = new();

        postCodeServiceMock
            .Setup(x => x.GetByPostcodeAsync(postCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        LocationService locationService = new(postCodeServiceMock.Object, loggerMock.Object);

        // Act
        Result<Location.Domain.Entities.Location> result =
            await locationService.GetByPostcodeAsync(postCode, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();

        // Verify mapping from PostCodeData to Location
        Location.Domain.Entities.Location expectedLocation = response.Result.Adapt<Location.Domain.Entities.Location>();
        result.Value.ShouldBeEquivalentTo(expectedLocation);
    }

    [Theory]
    [AutoData]
    public async Task GetByPostcodeAsync_WhenPostcodeNotFound_ReturnsNotFound(string postCode)
    {
        // Arrange
        Mock<IPostCodeIoService> postCodeServiceMock = new();
        Mock<ILogger<LocationService>> loggerMock = new();

        ApiException apiException = await ApiException.Create(
            new HttpRequestMessage(),
            HttpMethod.Get,
            new HttpResponseMessage(HttpStatusCode.NotFound),
            new RefitSettings());

        postCodeServiceMock
            .Setup(x => x.GetByPostcodeAsync(postCode, It.IsAny<CancellationToken>()))
            .ThrowsAsync(apiException);

        LocationService sut = new(postCodeServiceMock.Object, loggerMock.Object);

        // Act
        Result<Location.Domain.Entities.Location> result =
            await sut.GetByPostcodeAsync(postCode, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.Errors.ShouldContain($"Postcode: {postCode} not found");
    }

    [Theory]
    [AutoData]
    public async Task GetByPostcodeAsync_WhenApiError_ReturnsCriticalError(string postCode)
    {
        // Arrange
        Mock<IPostCodeIoService> postCodeServiceMock = new();
        Mock<ILogger<LocationService>> loggerMock = new();

        ApiException apiException = await ApiException.Create(
            new HttpRequestMessage(),
            HttpMethod.Get,
            new HttpResponseMessage(HttpStatusCode.InternalServerError),
            new RefitSettings());

        postCodeServiceMock
            .Setup(x => x.GetByPostcodeAsync(postCode, It.IsAny<CancellationToken>()))
            .ThrowsAsync(apiException);

        LocationService sut = new(postCodeServiceMock.Object, loggerMock.Object);

        // Act
        Result<Location.Domain.Entities.Location> result =
            await sut.GetByPostcodeAsync(postCode, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.CriticalError);
        result.Errors.ShouldContain("Unable to process postcode lookup. Please try again later.");
    }

    [Theory]
    [AutoData]
    public async Task GetByPostcodeAsync_WhenGeneralException_ReturnsCriticalError(string postCode)
    {
        // Arrange
        Mock<IPostCodeIoService> postCodeServiceMock = new();
        Mock<ILogger<LocationService>> loggerMock = new();

        postCodeServiceMock
            .Setup(x => x.GetByPostcodeAsync(postCode, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Test exception"));

        LocationService sut = new(postCodeServiceMock.Object, loggerMock.Object);

        // Act
        Result<Location.Domain.Entities.Location> result =
            await sut.GetByPostcodeAsync(postCode, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.CriticalError);
        result.Errors.ShouldContain("Unable to process postcode lookup. Please try again later.");
    }
}