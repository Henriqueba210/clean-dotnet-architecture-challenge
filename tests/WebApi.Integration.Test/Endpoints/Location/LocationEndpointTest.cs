using System.Net.Http.Json;

using Shouldly;

using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace WebApi.Integration.Test.Endpoints.Location;


public class LocationEndpointTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetLocationByPostcode_WithValidPostcode_ReturnsOk()
    {
        // Arrange
        var postcode = "SW1A 1AA";

        // Act
        var response = await _client.GetAsync($"/api/location/address/{postcode}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.ShouldBe("application/json");

        var location = await response.Content.ReadFromJsonAsync<global::Location.Domain.Entities.Location>();
        location.ShouldNotBeNull();
        location.Postcode.ShouldBe(postcode);
        location.Country.ShouldBe("England");
        location.Region.ShouldBe("London");
        location.AdminDistrict.ShouldBe("Westminster");
    }

    [Fact]
    public async Task GetLocationByPostcode_WithInvalidPostcode_ReturnsNotFound()
    {
        // Arrange
        const string postcode = "INVALID";

        // Act
        var response = await _client.GetAsync($"/api/location/address/{postcode}");
    
        // Ensure there is content before deserializing
        if (response.Content.Headers.ContentLength > 0)
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        
            // Assert with problem details
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            problem.ShouldNotBeNull();
            problem.Status.ShouldBe(404);
            problem.Type.ShouldBe("https://tools.ietf.org/html/rfc7231#section-6.5.4");
        }
        else
        {
            // Assert status code only
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}