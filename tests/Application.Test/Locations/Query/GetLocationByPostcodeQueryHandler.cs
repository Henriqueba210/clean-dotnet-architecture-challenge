namespace Application.Test.Locations.Query;

public class GetLocationByPostcodeQueryHandlerTests
{
    private readonly IFixture _fixture;
    private readonly GetLocationByPostcodeQueryHandler _handler;
    private readonly Mock<ILocationService> _locationServiceMock;

    public GetLocationByPostcodeQueryHandlerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _locationServiceMock = _fixture.Freeze<Mock<ILocationService>>();
        _handler = _fixture.Create<GetLocationByPostcodeQueryHandler>();
    }

    [Fact]
    public async Task Handle_WithValidPostcode_ShouldReturnCorrectDistance()
    {
        // Arrange
        GetLocationByPostcodeQuery query = new("SW1A 1AA");
        Location.Domain.Entities.Location location = new LocationBuilder()
            .WithPostcode(query.PostCode)
            .Build();

        _locationServiceMock
            .Setup(x => x.GetByPostcodeAsync(query.PostCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => Result.Success(location));

        // Act
        Result<Location.Domain.Entities.Location> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.DistanceToHeathrowAirport.ShouldNotBeNull();
        result.Value.DistanceToHeathrowAirport.Kilometers.ShouldBeInRange(23.0, 24.0);
        result.Value.DistanceToHeathrowAirport.Miles.ShouldBeInRange(14.0, 15.0);
        result.Value.Postcode.ShouldBe(query.PostCode);
    }

    [Fact]
    public async Task Handle_WithInvalidPostcode_ShouldReturnError()
    {
        // Arrange
        const string errorMessage = "Invalid postcode";
        GetLocationByPostcodeQuery? query = _fixture.Create<GetLocationByPostcodeQuery>();

        _locationServiceMock
            .Setup(x => x.GetByPostcodeAsync(query.PostCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => Result.Error(errorMessage));

        // Act
        Result<Location.Domain.Entities.Location>? result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
    }
}