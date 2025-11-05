using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Places;
using Domain.PlacePhotos;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Places;
using Xunit;

namespace Api.Tests.Integration.PlacePhotos;

public class PlacePhotoControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private Place _place = default!;
    private PlacePhoto _firstPhoto = default!;
    private PlacePhoto _secondPhoto = default!;

    private const string BaseRoute = "place-photos";

    public PlacePhotoControllerTests(IntegrationTestWebFactory factory) : base(factory) { }

    // -------------------------------
    // ✅ SUCCESSFUL TESTS
    // -------------------------------

    [Fact]
    public async Task ShouldGetPlacePhotoById()
    {
        var response = await Client.GetAsync($"{BaseRoute}/{_firstPhoto.Id.Value}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<PlacePhotoDto>();

        dto.Id.Should().Be(_firstPhoto.Id.Value);
        dto.Description.Should().Be(_firstPhoto.Description);
        dto.PlaceId.Should().Be(_place.Id.Value);
        dto.IsShown.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldGetAllPlacePhotos()
    {
        var response = await Client.GetAsync(BaseRoute);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var photos = await response.ToResponseModel<List<PlacePhotoDto>>();
        photos.Should().ContainSingle(p => p.Description == _firstPhoto.Description);
    }

    [Fact]
    public async Task ShouldCreatePlacePhoto()
    {
        var request = new CreatePlacePhotoDto(
            new byte[] { 0xAA, 0xBB, 0xCC },
            "New Photo",
            _place.Id.Value);

        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        response.IsSuccessStatusCode.Should().BeTrue();
        var dto = await response.ToResponseModel<PlacePhotoDto>();

        var dbEntity = (await Context.PlacePhotos.ToListAsync())
            .First(x => x.Id.Value == dto.Id);

        dbEntity.Description.Should().Be("New Photo");
        dbEntity.PlaceId.Should().Be(_place.Id);
        dbEntity.IsShown.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldUpdatePlacePhoto()
    {
        var updateRequest = new UpdatePlacePhotoDto(
            _firstPhoto.Id.Value,
            new byte[] { 0x10, 0x20 },
            "Updated Photo",
            false);

        var response = await Client.PutAsJsonAsync(BaseRoute, updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dto = await response.ToResponseModel<PlacePhotoDto>();
        dto.Description.Should().Be("Updated Photo");
        dto.IsShown.Should().BeFalse();

        var dbEntity = await Context.PlacePhotos.FindAsync(_firstPhoto.Id);
        dbEntity!.Description.Should().Be("Updated Photo");
        dbEntity.IsShown.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldDeletePlacePhoto()
    {
        var response = await Client.DeleteAsync($"{BaseRoute}/{_firstPhoto.Id.Value}");

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);

        var exists = await Context.PlacePhotos.AnyAsync(x => x.Id == _firstPhoto.Id);
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldToggleVisibility()
    {
        _firstPhoto.ChangeVisibility(false);
        await Context.SaveChangesAsync();

        _firstPhoto.IsShown.Should().BeFalse();
    }

    // -------------------------------
    // ❌ UNSUCCESSFUL TESTS
    // -------------------------------

    [Fact]
    public async Task ShouldReturnNotFound_WhenPhotoDoesNotExist()
    {
        var response = await Client.GetAsync($"{BaseRoute}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldNotCreatePhoto_WhenPlaceDoesNotExist()
    {
        var request = new CreatePlacePhotoDto(
            new byte[] { 0x01 },
            "Ghost Photo",
            Guid.NewGuid());

        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task ShouldNotCreatePhoto_WhenDescriptionIsInvalid(string? description)
    {
        var request = new CreatePlacePhotoDto(
            new byte[] { 0x01 },
            description!,
            _place.Id.Value);

        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task ShouldNotUpdatePhoto_WhenPhotoDoesNotExist()
    {
        var request = new UpdatePlacePhotoDto(
            Guid.NewGuid(),
            new byte[] { 0x22 },
            "No Photo",
            true);

        var response = await Client.PutAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task ShouldNotUpdatePhoto_WhenDescriptionIsInvalid(string? description)
    {
        var request = new UpdatePlacePhotoDto(
            _firstPhoto.Id.Value,
            new byte[] { 0x30 },
            description!,
            true);

        var response = await Client.PutAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task ShouldNotDeletePhoto_WhenPhotoDoesNotExist()
    {
        var response = await Client.DeleteAsync($"{BaseRoute}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task ShouldAllowSameDescriptionInDifferentPlaces()
    {
        var anotherPlace = PlacesData.SecondTestPlace();
        await Context.Places.AddAsync(anotherPlace);
        await SaveChangesAsync();

        var request = new CreatePlacePhotoDto(
            new byte[] { 0x50 },
            _firstPhoto.Description,
            anotherPlace.Id.Value);

        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ShouldNotCreatePhoto_WhenImageIsEmpty()
    {
        var request = new CreatePlacePhotoDto(
            Array.Empty<byte>(),
            "Empty Image",
            _place.Id.Value);

        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // -------------------------------
    // ⚙️ TEST SETUP / CLEANUP
    // -------------------------------

    public async Task InitializeAsync()
    {
        _place = PlacesData.FirstTestPlace();
        _firstPhoto = PlacePhoto.New(new byte[] { 0x01, 0x02 }, "Photo 1", _place.Id);
        _secondPhoto = PlacePhoto.New(new byte[] { 0x03, 0x04 }, "Photo 2", _place.Id);

        await Context.Places.AddAsync(_place);
        await Context.PlacePhotos.AddRangeAsync(_firstPhoto, _secondPhoto);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.PlacePhotos.RemoveRange(Context.PlacePhotos);
        Context.Places.RemoveRange(Context.Places);
        await SaveChangesAsync();
    }
}
