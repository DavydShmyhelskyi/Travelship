using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Cities;
using Domain.Countries;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Cities;
using Tests.Data.Countries;
using Xunit;

namespace Api.Tests.Integration.Cities;

public class CityControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private Country _country = default!;
    private City _firstTestCity = default!;
    private City _secondTestCity = default!;

    private const string BaseRoute = "cities";

    public CityControllerTests(IntegrationTestWebFactory factory) : base(factory) { }

    // -------------------------------
    // ✅ SUCCESSFUL TESTS
    // -------------------------------

    [Fact]
    public async Task ShouldGetCityById()
    {
        var response = await Client.GetAsync($"{BaseRoute}/{_firstTestCity.Id.Value}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<CityDto>();

        dto.Id.Should().Be(_firstTestCity.Id.Value);
        dto.Title.Should().Be(_firstTestCity.Title);
        dto.CountryId.Should().Be(_country.Id.Value);
    }

    [Fact]
    public async Task ShouldGetAllCities()
    {
        var response = await Client.GetAsync(BaseRoute);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var cities = await response.ToResponseModel<List<CityDto>>();
        cities.Should().ContainSingle(c => c.Title == _firstTestCity.Title);
    }

    [Fact]
    public async Task ShouldCreateCity()
    {
        var request = new CreateCityDto("New City", _country.Id.Value);

        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        response.IsSuccessStatusCode.Should().BeTrue();
        var dto = await response.ToResponseModel<CityDto>();

        var dbEntity = (await Context.Cities.ToListAsync())
            .First(x => x.Id.Value == dto.Id);

        dbEntity.Title.Should().Be("New City");
        dbEntity.CountryId.Should().Be(_country.Id);
    }

    [Fact]
    public async Task ShouldUpdateCity()
    {
        var updateRequest = new UpdateCityDto(
            _firstTestCity.Id.Value, "Updated City", _country.Id.Value);

        var response = await Client.PutAsJsonAsync(BaseRoute, updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dto = await response.ToResponseModel<CityDto>();
        dto.Title.Should().Be("Updated City");

        var dbEntity = await Context.Cities.FindAsync(_firstTestCity.Id);
        dbEntity!.Title.Should().Be("Updated City");
    }

    [Fact]
    public async Task ShouldDeleteCity()
    {
        var response = await Client.DeleteAsync($"{BaseRoute}/{_firstTestCity.Id.Value}");

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);

        var exists = await Context.Cities.AnyAsync(x => x.Id == _firstTestCity.Id);
        exists.Should().BeFalse();
    }

    // -------------------------------
    // ❌ UNSUCCESSFUL TESTS
    // -------------------------------

    [Fact]
    public async Task ShouldReturnNotFound_WhenCityDoesNotExist()
    {
        var response = await Client.GetAsync($"{BaseRoute}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldNotCreateCity_WhenTitleAlreadyExistsInSameCountry()
    {
        var request = new CreateCityDto(_firstTestCity.Title, _country.Id.Value);
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task ShouldNotCreateCity_WhenTitleIsInvalid(string? title)
    {
        var request = new CreateCityDto(title!, _country.Id.Value);
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task ShouldNotCreateCity_WhenCountryDoesNotExist()
    {
        var nonExistingCountryId = Guid.NewGuid();
        var request = new CreateCityDto("Ghost City", nonExistingCountryId);

        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task ShouldNotUpdateCity_WhenTitleIsInvalid(string? title)
    {
        var request = new UpdateCityDto(_firstTestCity.Id.Value, title!, _country.Id.Value);
        var response = await Client.PutAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task ShouldNotUpdateCity_WhenCityDoesNotExist()
    {
        var request = new UpdateCityDto(Guid.NewGuid(), "NoCity", _country.Id.Value);
        var response = await Client.PutAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldNotDeleteCity_WhenCityDoesNotExist()
    {
        var response = await Client.DeleteAsync($"{BaseRoute}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldBeCaseInsensitiveForDuplicatesWithinCountry()
    {
        var request = new CreateCityDto(_firstTestCity.Title.ToUpperInvariant(), _country.Id.Value);
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task ShouldAllowSameCityNameInDifferentCountries()
    {
        var otherCountry = CountriesData.SecondTestCountry();
        await Context.Countries.AddAsync(otherCountry);
        await SaveChangesAsync();

        var request = new CreateCityDto(_firstTestCity.Title, otherCountry.Id.Value);
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // -------------------------------
    // ⚙️ TEST SETUP / CLEANUP
    // -------------------------------

    public async Task InitializeAsync()
    {
        // 1️⃣ Додаємо країну
        _country = CountriesData.FirstTestCountry();
        await Context.Countries.AddAsync(_country);
        await SaveChangesAsync();

        // 2️⃣ Створюємо міста для цієї країни
        _firstTestCity = CitiesData.FirstTestCity(_country);
        _secondTestCity = CitiesData.SecondTestCity(_country);

        await Context.Cities.AddRangeAsync(_firstTestCity, _secondTestCity);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Cities.RemoveRange(Context.Cities);
        Context.Countries.RemoveRange(Context.Countries);
        await SaveChangesAsync();
    }
}
