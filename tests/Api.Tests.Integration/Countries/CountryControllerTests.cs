using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Countries;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Countries;
using Xunit;

namespace Api.Tests.Integration.Countries;

public class CountryControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Country _firstTestCountry = CountriesData.FirstTestCountry();
    private readonly Country _secondTestCountry = CountriesData.SecondTestCountry();

    private const string BaseRoute = "countries";

    public CountryControllerTests(IntegrationTestWebFactory factory) : base(factory) { }

    // -------------------------------
    // ✅ SUCCESSFUL TESTS
    // -------------------------------

    [Fact]
    public async Task ShouldGetCountryById()
    {
        var response = await Client.GetAsync($"{BaseRoute}/{_firstTestCountry.Id.Value}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<CountryDto>();
        dto.Id.Should().Be(_firstTestCountry.Id.Value);
        dto.Title.Should().Be(_firstTestCountry.Title);
    }

    [Fact]
    public async Task ShouldGetAllCountries()
    {
        var response = await Client.GetAsync(BaseRoute);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var countries = await response.ToResponseModel<List<CountryDto>>();
        countries.Should().ContainSingle(c => c.Title == _firstTestCountry.Title);
    }

    [Fact]
    public async Task ShouldCreateCountry()
    {
        var request = new CreateCountryDto("New Country");

        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        response.IsSuccessStatusCode.Should().BeTrue();
        var dto = await response.ToResponseModel<CountryDto>();

        var dbEntity = (await Context.Countries.ToListAsync())
            .First(x => x.Id.Value == dto.Id);
        dbEntity.Title.Should().Be("New Country");
    }

    [Fact]
    public async Task ShouldUpdateCountry()
    {
        var updateRequest = new UpdateCountryDto(_firstTestCountry.Id.Value, "Updated Country");

        var response = await Client.PutAsJsonAsync(BaseRoute, updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dto = await response.ToResponseModel<CountryDto>();
        dto.Title.Should().Be("Updated Country");

        var dbEntity = await Context.Countries.FindAsync(_firstTestCountry.Id);
        dbEntity!.Title.Should().Be("Updated Country");
    }

    [Fact]
    public async Task ShouldDeleteCountry()
    {
        var response = await Client.DeleteAsync($"{BaseRoute}/{_firstTestCountry.Id.Value}");

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);

        var exists = await Context.Countries.AnyAsync(x => x.Id == _firstTestCountry.Id);
        exists.Should().BeFalse();
    }


    // UNSUCCESSFUL TESTS


    [Fact]
    public async Task ShouldReturnNotFound_WhenCountryDoesNotExist()
    {
        var notExistingId = Guid.NewGuid();
        var response = await Client.GetAsync($"{BaseRoute}/{notExistingId}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldNotCreateCountry_WhenTitleAlreadyExists()
    {
        var request = new CreateCountryDto(_firstTestCountry.Title);
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task ShouldNotCreateCountry_WhenTitleIsInvalid(string? title)
    {
        var request = new CreateCountryDto(title!);
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);

    }

    [Fact]
    public async Task ShouldNotUpdateCountry_WhenCountryDoesNotExist()
    {
        var request = new UpdateCountryDto(Guid.NewGuid(), "Ghostland");
        var response = await Client.PutAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task ShouldNotUpdateCountry_WhenTitleIsInvalid(string? title)
    {
        var request = new UpdateCountryDto(_firstTestCountry.Id.Value, title!);
        var response = await Client.PutAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);

    }

    [Fact]
    public async Task ShouldNotDeleteCountry_WhenCountryDoesNotExist()
    {
        var response = await Client.DeleteAsync($"{BaseRoute}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldNotCreateCountry_WhenTitleIsTooLong()
    {
        var longTitle = new string('X', 300);
        var response = await Client.PostAsJsonAsync(BaseRoute, new CreateCountryDto(longTitle));
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);

    }

    [Fact]
    public async Task ShouldBeCaseInsensitiveForDuplicates()
    {
        var request = new CreateCountryDto(_firstTestCountry.Title.ToUpperInvariant());
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // -------------------------------
    // ⚙️ TEST SETUP / CLEANUP
    // -------------------------------

    public async Task InitializeAsync()
    {
        await Context.Countries.AddAsync(_firstTestCountry);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Countries.RemoveRange(Context.Countries);
        await SaveChangesAsync();
    }
}
