using Api.Dtos;
using Domain.Places;
using Domain.Travels;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Tests.Common;
using Tests.Data.Cities;
using Tests.Data.Countries;
using Tests.Data.Places;
using Tests.Data.Roles;
using Tests.Data.Users;
using Xunit;

namespace Api.Tests.Integration.Travels;

public class TravelControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private User _user = default!;
    private Place _place = default!;
    private Travel _travel = default!;

    private const string BaseRoute = "travels";

    public TravelControllerTests(IntegrationTestWebFactory factory) : base(factory) { }

    // -------------------------------
    // ✅ SUCCESSFUL TESTS
    // -------------------------------

    [Fact]
    public async Task ShouldGetAllTravels()
    {
        var response = await Client.GetAsync(BaseRoute);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await response.ToResponseModel<List<TravelDto>>();

        list.Should().ContainSingle(t => t.Title == _travel.Title);
    }

    [Fact]
    public async Task ShouldGetTravelById()
    {
        var response = await Client.GetAsync($"{BaseRoute}/{_travel.Id.Value}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dto = await response.ToResponseModel<TravelDto>();
        dto.Id.Should().Be(_travel.Id.Value);
        dto.Title.Should().Be(_travel.Title);
    }

    [Fact]
    public async Task ShouldCreateTravel()
    {
        var request = new CreateTravelDto(
            Title: "New Travel",
            StartDate: DateTime.UtcNow.AddDays(1),
            EndDate: DateTime.UtcNow.AddDays(5),
            Description: "Trip test",
            Image: null,
            UserId: _user.Id.Value,
            Members: new List<Guid> { _user.Id.Value },
            Places: new List<Guid> { _place.Id.Value }
        );

        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.IsSuccessStatusCode.Should().BeTrue($"Actual status: {response.StatusCode}, content: {await response.Content.ReadAsStringAsync()}");

        var dto = await response.ToResponseModel<TravelDto>();
        dto.Title.Should().Be("New Travel");
        dto.Description.Should().Be("Trip test");
        dto.UserId.Should().Be(_user.Id.Value);

        var dbEntity = Context.Travels
            .AsEnumerable()
            .FirstOrDefault(t => t.Id.Value == dto.Id);

        dbEntity.Should().NotBeNull();
        dbEntity!.Title.Should().Be("New Travel");
    }

    [Fact]
    public async Task ShouldUpdateTravel()
    {
        var update = new UpdateTravelDto(
            Id: _travel.Id.Value,
            Title: "Updated Travel",
            StartDate: _travel.StartDate,
            EndDate: _travel.EndDate.AddDays(2),
            Description: "Updated description",
            Image: null,
            IsDone: true,
            Members: new List<Guid> { _user.Id.Value },
            Places: new List<Guid> { _place.Id.Value }
        );

        var response = await Client.PutAsJsonAsync(BaseRoute, update);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dto = await response.ToResponseModel<TravelDto>();
        dto.Title.Should().Be("Updated Travel");
        dto.Description.Should().Be("Updated description");
        dto.IsDone.Should().BeTrue();

        var dbEntity = Context.Travels
            .AsEnumerable()
            .First(t => t.Id == _travel.Id);
        dbEntity.Title.Should().Be("Updated Travel");
    }

    [Fact]
    public async Task ShouldDeleteTravel()
    {
        var response = await Client.DeleteAsync($"{BaseRoute}/{_travel.Id.Value}");
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);

        var exists = await Context.Travels.AnyAsync(t => t.Id == _travel.Id);
        exists.Should().BeFalse();
    }

    // -------------------------------
    // ❌ UNSUCCESSFUL TESTS
    // -------------------------------

    [Fact]
    public async Task ShouldReturnNotFound_WhenTravelDoesNotExist()
    {
        var response = await Client.GetAsync($"{BaseRoute}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldNotCreateTravel_WhenMembersListIsEmpty()
    {
        var request = new CreateTravelDto(
            Title: "No members travel",
            StartDate: DateTime.UtcNow,
            EndDate: DateTime.UtcNow.AddDays(3),
            Description: "Travel without members",
            Image: null,
            UserId: _user.Id.Value,   
            Members: new List<Guid>(),
            Places: new List<Guid> { _place.Id.Value }
        );

        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
    }


    [Fact]
    public async Task ShouldNotCreateTravel_WhenPlaceDoesNotExist()
    {
        var request = new CreateTravelDto(
            Title: "Invalid place travel",
            StartDate: DateTime.UtcNow,
            EndDate: DateTime.UtcNow.AddDays(2),
            Description: "Missing place",
            Image: null,
            UserId: _user.Id.Value,
            Members: new List<Guid> { _user.Id.Value },
            Places: new List<Guid>()
        );

        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task ShouldNotCreateTravel_WhenTitleIsInvalid(string? title)
    {
        var request = new CreateTravelDto(
            Title: title!,
            StartDate: DateTime.UtcNow,
            EndDate: DateTime.UtcNow.AddDays(1),
            Description: "Invalid title test",
            Image: null,
            UserId: _user.Id.Value,
            Members: new List<Guid> { _user.Id.Value },
            Places: new List<Guid> { _place.Id.Value }
        );

        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task ShouldNotUpdateTravel_WhenTravelDoesNotExist()
    {
        var request = new UpdateTravelDto(
            Id: Guid.NewGuid(),
            Title: "Nonexistent Travel",
            StartDate: DateTime.UtcNow,
            EndDate: DateTime.UtcNow.AddDays(3),
            Description: "No such travel",
            Image: null,
            IsDone: false,
            Members: new List<Guid> { _user.Id.Value },
            Places: new List<Guid> { _place.Id.Value }
        );

        var response = await Client.PutAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldNotDeleteTravel_WhenTravelDoesNotExist()
    {
        var response = await Client.DeleteAsync($"{BaseRoute}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // -------------------------------
    // ⚙️ SETUP / CLEANUP
    // -------------------------------

    public async Task InitializeAsync()
    {
        Context.Countries.RemoveRange(Context.Countries);
        Context.Cities.RemoveRange(Context.Cities);
        Context.Roles.RemoveRange(Context.Roles);
        Context.Users.RemoveRange(Context.Users);
        Context.Places.RemoveRange(Context.Places);
        Context.Travels.RemoveRange(Context.Travels);
        await Context.SaveChangesAsync();

        var country = CountriesData.FirstTestCountry();
        var city = CitiesData.FirstTestCity(country);
        var role = RolesData.FirstTestRole();

        _user = UsersData.FirstTestUser(role, city);
        _place = PlacesData.FirstTestPlace();

        _travel = Travel.New(
            TravelId.New(),
            "Test Travel",
            DateTime.UtcNow.AddDays(-1),
            DateTime.UtcNow.AddDays(3),
            "Simple test travel",
            null,
            _user.Id,
            new List<UserTravel>(),
            new List<TravelPlace>());

        await Context.Countries.AddAsync(country);
        await Context.Cities.AddAsync(city);
        await Context.Roles.AddAsync(role);
        await Context.Users.AddAsync(_user);
        await Context.Places.AddAsync(_place);
        await Context.Travels.AddAsync(_travel);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.TravelPlaces.RemoveRange(Context.TravelPlaces);
        Context.UserTravels.RemoveRange(Context.UserTravels);
        Context.Travels.RemoveRange(Context.Travels);
        Context.Places.RemoveRange(Context.Places);
        Context.Users.RemoveRange(Context.Users);
        await SaveChangesAsync();
    }
}
