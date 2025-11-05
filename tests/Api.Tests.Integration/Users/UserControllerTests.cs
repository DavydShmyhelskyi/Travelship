using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Users;
using Domain.Cities;
using Domain.Countries;
using Domain.Roles;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Users;
using Tests.Data.Cities;
using Tests.Data.Countries;
using Tests.Data.Roles;
using Xunit;

namespace Api.Tests.Integration.Users;

public class UserControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private Country _country = default!;
    private City _city = default!;
    private Role _role = default!;
    private User _firstTestUser = default!;
    private User _secondTestUser = default!;

    private const string BaseRoute = "users";

    public UserControllerTests(IntegrationTestWebFactory factory) : base(factory) { }

    // -------------------------------
    // ✅ SUCCESSFUL TESTS
    // -------------------------------

    [Fact]
    public async Task ShouldGetUserById()
    {
        var response = await Client.GetAsync($"{BaseRoute}/{_firstTestUser.Id.Value}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<UserDto>();

        dto.Id.Should().Be(_firstTestUser.Id.Value);
        dto.Email.Should().Be(_firstTestUser.Email);
        dto.NickName.Should().Be(_firstTestUser.NickName);
        dto.RoleId.Should().Be(_role.Id.Value);
        dto.CityId.Should().Be(_city.Id.Value);
    }

    [Fact]
    public async Task ShouldGetAllUsers()
    {
        var response = await Client.GetAsync(BaseRoute);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var users = await response.ToResponseModel<List<UserDto>>();
        users.Should().ContainSingle(u => u.Email == _firstTestUser.Email);
    }

    [Fact]
    public async Task ShouldCreateUser()
    {
        var request = new CreateUserDto(
            "NewUser",
            null,
            "newuser@gmail.com",
            "Password123!",
            _role.Id.Value,
            _city.Id.Value);

        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        response.IsSuccessStatusCode.Should().BeTrue();
        var dto = await response.ToResponseModel<UserDto>();

        var dbEntity = (await Context.Users.ToListAsync())
            .First(x => x.Id.Value == dto.Id);

        dbEntity.NickName.Should().Be("NewUser");
        dbEntity.Email.Should().Be("newuser@gmail.com");
        dbEntity.RoleId.Should().Be(_role.Id);
        dbEntity.CityId.Should().Be(_city.Id);
    }

    [Fact]
    public async Task ShouldUpdateUser()
    {
        var updateRequest = new UpdateUserDto(
            _firstTestUser.Id.Value,
            "UpdatedUser",
            null,
            _firstTestUser.Email,
            _role.Id.Value,
            _city.Id.Value);

        var response = await Client.PutAsJsonAsync(BaseRoute, updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<UserDto>();
        dto.NickName.Should().Be("UpdatedUser");

        var dbEntity = await Context.Users.FindAsync(_firstTestUser.Id);
        dbEntity!.NickName.Should().Be("UpdatedUser");
    }

    [Fact]
    public async Task ShouldDeleteUser()
    {
        var response = await Client.DeleteAsync($"{BaseRoute}/{_firstTestUser.Id.Value}");

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);

        var exists = await Context.Users.AnyAsync(x => x.Id == _firstTestUser.Id);
        exists.Should().BeFalse();
    }

    // -------------------------------
    // ❌ UNSUCCESSFUL TESTS
    // -------------------------------

    [Fact]
    public async Task ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        var response = await Client.GetAsync($"{BaseRoute}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldNotCreateUser_WhenEmailAlreadyExists()
    {
        var request = new CreateUserDto(
            "DuplicateUser",
            null,
            _firstTestUser.Email,
            "Password123!",
            _role.Id.Value,
            _city.Id.Value);

        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task ShouldNotCreateUser_WhenNickNameIsInvalid(string? nick)
    {
        var request = new CreateUserDto(
            nick!,
            null,
            "invalidnick@gmail.com",
            "Password123!",
            _role.Id.Value,
            _city.Id.Value);

        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task ShouldNotCreateUser_WhenRoleDoesNotExist()
    {
        var request = new CreateUserDto(
            "GhostUser",
            null,
            "ghostuser@gmail.com",
            "GhostPass!",
            Guid.NewGuid(),
            _city.Id.Value);

        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldNotCreateUser_WhenCityDoesNotExist()
    {
        var request = new CreateUserDto(
            "NoCityUser",
            null,
            "nocity@gmail.com",
            "Pass123!",
            _role.Id.Value,
            Guid.NewGuid());

        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldNotUpdateUser_WhenUserDoesNotExist()
    {
        var request = new UpdateUserDto(
            Guid.NewGuid(),
            "Ghost",
            null,
            "ghost@gmail.com",
            _role.Id.Value,
            _city.Id.Value);

        var response = await Client.PutAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task ShouldNotUpdateUser_WhenNickNameIsInvalid(string? nick)
    {
        var request = new UpdateUserDto(
            _firstTestUser.Id.Value,
            nick!,
            null,
            _firstTestUser.Email,
            _role.Id.Value,
            _city.Id.Value);

        var response = await Client.PutAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task ShouldBeCaseInsensitiveForDuplicateEmails()
    {
        var request = new CreateUserDto(
            "CaseInsensitiveUser",
            null,
            _firstTestUser.Email.ToUpperInvariant(),
            "Password!",
            _role.Id.Value,
            _city.Id.Value);

        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // -------------------------------
    // ⚙️ TEST SETUP / CLEANUP
    // -------------------------------

    public async Task InitializeAsync()
    {
        Context.Users.RemoveRange(Context.Users);
        Context.Cities.RemoveRange(Context.Cities);
        Context.Countries.RemoveRange(Context.Countries);
        Context.Roles.RemoveRange(Context.Roles);
        await SaveChangesAsync();

        _country = CountriesData.FirstTestCountry();
        await Context.Countries.AddAsync(_country);
        await SaveChangesAsync();

        _city = CitiesData.FirstTestCity(_country);
        await Context.Cities.AddAsync(_city);
        await SaveChangesAsync();

        _role = RolesData.FirstTestRole();
        await Context.Roles.AddAsync(_role);
        await SaveChangesAsync();

        // 👇 передаємо role і city в UsersData
        _firstTestUser = UsersData.FirstTestUser(_role, _city);
        _secondTestUser = UsersData.SecondTestUser(_role, _city);

        await Context.Users.AddRangeAsync(_firstTestUser, _secondTestUser);
        await SaveChangesAsync();
    }





    public async Task DisposeAsync()
    {
        Context.Users.RemoveRange(Context.Users);
        Context.Roles.RemoveRange(Context.Roles);
        Context.Cities.RemoveRange(Context.Cities);
        Context.Countries.RemoveRange(Context.Countries);
        await SaveChangesAsync();
    }
}
