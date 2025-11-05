using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Places;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Places;
using Xunit;

namespace Api.Tests.Integration.Places;

public class PlaceControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Place _firstTestPlace = PlacesData.FirstTestPlace();
    private readonly Place _secondTestPlace = PlacesData.SecondTestPlace();

    private const string BaseRoute = "places";

    public PlaceControllerTests(IntegrationTestWebFactory factory) : base(factory) { }

    // -------------------------------
    // ✅ SUCCESSFUL TESTS
    // -------------------------------

    [Fact]
    public async Task ShouldGetPlaceById()
    {
        var response = await Client.GetAsync($"{BaseRoute}/{_firstTestPlace.Id.Value}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<PlaceDto>();

        dto.Id.Should().Be(_firstTestPlace.Id.Value);
        dto.Title.Should().Be(_firstTestPlace.Title);
        dto.Latitude.Should().Be(_firstTestPlace.Latitude);
        dto.Longitude.Should().Be(_firstTestPlace.Longitude);
    }

    [Fact]
    public async Task ShouldGetAllPlaces()
    {
        var response = await Client.GetAsync(BaseRoute);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var places = await response.ToResponseModel<List<PlaceDto>>();
        places.Should().ContainSingle(p => p.Title == _firstTestPlace.Title);
    }

    [Fact]
    public async Task ShouldCreatePlace()
    {
        var request = new CreatePlaceDto("New Place", 50.4501, 30.5234);

        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        response.IsSuccessStatusCode.Should().BeTrue();
        var dto = await response.ToResponseModel<PlaceDto>();

        var dbEntity = (await Context.Places.ToListAsync())
            .First(x => x.Id.Value == dto.Id);

        dbEntity.Title.Should().Be("New Place");
        dbEntity.Latitude.Should().Be(50.4501);
        dbEntity.Longitude.Should().Be(30.5234);
    }

    [Fact]
    public async Task ShouldUpdatePlace()
    {
        var updateRequest = new UpdatePlaceDto(
            _firstTestPlace.Id.Value,
            "Updated Place",
            51.5074,
            -0.1278);

        var response = await Client.PutAsJsonAsync(BaseRoute, updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dto = await response.ToResponseModel<PlaceDto>();
        dto.Title.Should().Be("Updated Place");

        var dbEntity = await Context.Places.FindAsync(_firstTestPlace.Id);
        dbEntity!.Title.Should().Be("Updated Place");
    }

    [Fact]
    public async Task ShouldDeletePlace()
    {
        var response = await Client.DeleteAsync($"{BaseRoute}/{_firstTestPlace.Id.Value}");

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);

        var exists = await Context.Places.AnyAsync(x => x.Id == _firstTestPlace.Id);
        exists.Should().BeFalse();
    }

    // -------------------------------
    // ❌ UNSUCCESSFUL TESTS
    // -------------------------------

    [Fact]
    public async Task ShouldReturnNotFound_WhenPlaceDoesNotExist()
    {
        var response = await Client.GetAsync($"{BaseRoute}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldNotCreatePlace_WhenTitleAlreadyExists()
    {
        var request = new CreatePlaceDto(_firstTestPlace.Title, _firstTestPlace.Latitude, _firstTestPlace.Longitude);
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task ShouldNotCreatePlace_WhenTitleIsInvalid(string? title)
    {
        var request = new CreatePlaceDto(title!, 10.0, 20.0);
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task ShouldNotUpdatePlace_WhenPlaceDoesNotExist()
    {
        var request = new UpdatePlaceDto(Guid.NewGuid(), "Ghost Place", 0, 0);
        var response = await Client.PutAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task ShouldNotUpdatePlace_WhenTitleIsInvalid(string? title)
    {
        var request = new UpdatePlaceDto(_firstTestPlace.Id.Value, title!, 10, 20);
        var response = await Client.PutAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task ShouldNotDeletePlace_WhenPlaceDoesNotExist()
    {
        var response = await Client.DeleteAsync($"{BaseRoute}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldBeCaseInsensitiveForDuplicates()
    {
        var request = new CreatePlaceDto(_firstTestPlace.Title.ToUpperInvariant(), _firstTestPlace.Latitude, _firstTestPlace.Longitude);
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // -------------------------------
    // ⚙️ TEST SETUP / CLEANUP
    // -------------------------------

    public async Task InitializeAsync()
    {
        await Context.Places.AddAsync(_firstTestPlace);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Places.RemoveRange(Context.Places);
        await SaveChangesAsync();
    }
}
