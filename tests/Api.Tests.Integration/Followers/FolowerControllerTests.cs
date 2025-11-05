using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Users;
using Domain.Followers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Users;
using Tests.Data.Cities;
using Tests.Data.Countries;
using Tests.Data.Roles;
using Xunit;

namespace Api.Tests.Integration.Followers;

public class FollowerControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private User _followerUser = default!;
    private User _followedUser = default!;
    private Follower _existingFollower = default!;

    private const string BaseRoute = "followers";

    public FollowerControllerTests(IntegrationTestWebFactory factory) : base(factory) { }

    // ✅ SUCCESSFUL TESTS
    // -------------------------------

    [Fact]
    public async Task ShouldGetFollowerByIds()
    {
        var response = await Client.GetAsync($"{BaseRoute}/{_followerUser.Id.Value}/{_followedUser.Id.Value}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<FollowerDto>();

        dto.FollowerUserId.Should().Be(_followerUser.Id.Value);
        dto.FollowedUserId.Should().Be(_followedUser.Id.Value);
    }

    [Fact]
    public async Task ShouldCreateFollower()
    {
        var request = new CreateFollowerDto(
            _followedUser.Id.Value,
            _followerUser.Id.Value);

        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        response.IsSuccessStatusCode.Should().BeTrue();
        var dto = await response.ToResponseModel<FollowerDto>();

        dto.FollowerUserId.Should().Be(_followedUser.Id.Value);
        dto.FollowedUserId.Should().Be(_followerUser.Id.Value);

        var dbEntity = await Context.Followers.FirstOrDefaultAsync(f =>
            f.FollowerUserId == _followedUser.Id &&
            f.FollowedUserId == _followerUser.Id);

        dbEntity.Should().NotBeNull();
    }




    [Fact]
    public async Task ShouldDeleteFollower()
    {
        var response = await Client.DeleteAsync($"{BaseRoute}/{_followerUser.Id.Value}/{_followedUser.Id.Value}");
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);

        var exists = await Context.Followers.AnyAsync(
            f => f.FollowerUserId == _followerUser.Id && f.FollowedUserId == _followedUser.Id);
        exists.Should().BeFalse();
    }

    // ❌ UNSUCCESSFUL TESTS
    // -------------------------------

    [Fact]
    public async Task ShouldNotCreateFollower_WhenAlreadyExists()
    {
        var request = new CreateFollowerDto(_followerUser.Id.Value, _followedUser.Id.Value);
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task ShouldNotCreateFollower_WhenFollowerDoesNotExist()
    {
        var request = new CreateFollowerDto(Guid.NewGuid(), _followedUser.Id.Value);
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldNotCreateFollower_WhenFollowedDoesNotExist()
    {
        var request = new CreateFollowerDto(_followerUser.Id.Value, Guid.NewGuid());
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldNotCreateFollower_WhenFollowerEqualsFollowed()
    {
        var request = new CreateFollowerDto(_followerUser.Id.Value, _followerUser.Id.Value);
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldReturnNotFound_WhenFollowerNotExistsInGet()
    {
        var response = await Client.GetAsync($"{BaseRoute}/{Guid.NewGuid()}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ⚙️ TEST SETUP / CLEANUP
    // -------------------------------

    public async Task InitializeAsync()
    {
        Context.Followers.RemoveRange(Context.Followers);
        Context.Users.RemoveRange(Context.Users);
        Context.Roles.RemoveRange(Context.Roles);
        Context.Cities.RemoveRange(Context.Cities);
        Context.Countries.RemoveRange(Context.Countries);
        await SaveChangesAsync();

        var country1 = CountriesData.FirstTestCountry();
        var city1 = CitiesData.FirstTestCity(country1);
        var role1 = RolesData.FirstTestRole();
        _followerUser = UsersData.FirstTestUser(role1, city1);

        var country2 = CountriesData.SecondTestCountry();
        var city2 = CitiesData.SecondTestCity(country2);
        var role2 = RolesData.SecondTestRole();
        _followedUser = UsersData.SecondTestUser(role2, city2);

        _existingFollower = Follower.New(_followerUser.Id, _followedUser.Id);

        await Context.Countries.AddRangeAsync(country1, country2);
        await Context.Cities.AddRangeAsync(city1, city2);
        await Context.Roles.AddRangeAsync(role1, role2);
        await Context.Users.AddRangeAsync(_followerUser, _followedUser);
        await Context.Followers.AddAsync(_existingFollower);
        await SaveChangesAsync();
    }


    public async Task DisposeAsync()
    {
        Context.Followers.RemoveRange(Context.Followers);
        Context.Users.RemoveRange(Context.Users);
        Context.Roles.RemoveRange(Context.Roles);
        Context.Cities.RemoveRange(Context.Cities);
        Context.Countries.RemoveRange(Context.Countries);
        await SaveChangesAsync();
    }
}
