using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Roles;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Roles;
using Xunit;

namespace Api.Tests.Integration.Roles;

public class RoleControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _firstTestRole = RolesData.FirstTestRole();
    private readonly Role _secondTestRole = RolesData.SecondTestRole();

    private const string BaseRoute = "roles";

    public RoleControllerTests(IntegrationTestWebFactory factory) : base(factory) { }

    // -------------------------------
    // ✅ SUCCESSFUL TESTS
    // -------------------------------

    [Fact]
    public async Task ShouldGetRoleById()
    {
        var response = await Client.GetAsync($"{BaseRoute}/{_firstTestRole.Id.Value}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<RoleDto>();

        dto.Id.Should().Be(_firstTestRole.Id.Value);
        dto.Title.Should().Be(_firstTestRole.Title);
    }

    [Fact]
    public async Task ShouldGetAllRoles()
    {
        var response = await Client.GetAsync(BaseRoute);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var roles = await response.ToResponseModel<List<RoleDto>>();
        roles.Should().ContainSingle(r => r.Title == _firstTestRole.Title);
    }

    [Fact]
    public async Task ShouldCreateRole()
    {
        var request = new CreateRoleDto("New Role");

        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        response.IsSuccessStatusCode.Should().BeTrue();
        var dto = await response.ToResponseModel<RoleDto>();

        var dbEntity = (await Context.Roles.ToListAsync())
            .First(x => x.Id.Value == dto.Id);
        dbEntity.Title.Should().Be("New Role");
    }

    [Fact]
    public async Task ShouldUpdateRole()
    {
        var request = new UpdateRoleDto(_firstTestRole.Id.Value, "Updated Role");

        var response = await Client.PutAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dto = await response.ToResponseModel<RoleDto>();
        dto.Title.Should().Be("Updated Role");

        var dbEntity = await Context.Roles.FindAsync(_firstTestRole.Id);
        dbEntity!.Title.Should().Be("Updated Role");
    }

    [Fact]
    public async Task ShouldDeleteRole()
    {
        var response = await Client.DeleteAsync($"{BaseRoute}/{_firstTestRole.Id.Value}");

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);

        var exists = await Context.Roles.AnyAsync(x => x.Id == _firstTestRole.Id);
        exists.Should().BeFalse();
    }

    // -------------------------------
    // ❌ UNSUCCESSFUL TESTS
    // -------------------------------

    [Fact]
    public async Task ShouldReturnNotFound_WhenRoleDoesNotExist()
    {
        var response = await Client.GetAsync($"{BaseRoute}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldNotCreateRole_WhenTitleAlreadyExists()
    {
        var request = new CreateRoleDto(_firstTestRole.Title);
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task ShouldNotCreateRole_WhenTitleIsInvalid(string? title)
    {
        var request = new CreateRoleDto(title!);
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task ShouldNotUpdateRole_WhenRoleDoesNotExist()
    {
        var request = new UpdateRoleDto(Guid.NewGuid(), "Ghost Role");
        var response = await Client.PutAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task ShouldNotUpdateRole_WhenTitleIsInvalid(string? title)
    {
        var request = new UpdateRoleDto(_firstTestRole.Id.Value, title!);
        var response = await Client.PutAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task ShouldNotDeleteRole_WhenRoleDoesNotExist()
    {
        var response = await Client.DeleteAsync($"{BaseRoute}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldNotCreateRole_WhenTitleIsTooLong()
    {
        var longTitle = new string('R', 260);
        var request = new CreateRoleDto(longTitle);

        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task ShouldBeCaseInsensitiveForDuplicates()
    {
        var request = new CreateRoleDto(_firstTestRole.Title.ToUpperInvariant());
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // -------------------------------
    // ⚙️ TEST SETUP / CLEANUP
    // -------------------------------

    public async Task InitializeAsync()
    {
        await Context.Roles.AddAsync(_firstTestRole);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Roles.RemoveRange(Context.Roles);
        await SaveChangesAsync();
    }
}
