using Api.Dtos;
using Domain.Feedbacks;
using Domain.Places;
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

namespace Api.Tests.Integration.Feedbacks;

public class FeedbackControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private User _user = default!;
    private Place _place = default!;
    private Feedback _firstFeedback = default!;
    private Feedback _secondFeedback = default!;

    private const string BaseRoute = "feedbacks";

    public FeedbackControllerTests(IntegrationTestWebFactory factory) : base(factory) { }

    // -------------------------------
    // ✅ SUCCESSFUL TESTS
    // -------------------------------

    [Fact]
    public async Task ShouldGetAllFeedbacks()
    {
        var response = await Client.GetAsync(BaseRoute);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await response.ToResponseModel<List<FeedbackDto>>();
        list.Should().ContainSingle(f => f.Comment == _firstFeedback.Comment);
    }

    [Fact]
    public async Task ShouldGetFeedbackById()
    {
        var response = await Client.GetAsync($"{BaseRoute}/{_firstFeedback.Id.Value}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<FeedbackDto>();

        dto.Id.Should().Be(_firstFeedback.Id.Value);
        dto.Comment.Should().Be(_firstFeedback.Comment);
        dto.Rating.Should().Be(_firstFeedback.Rating);
        dto.UserId.Should().Be(_user.Id.Value);
        dto.PlaceId.Should().Be(_place.Id.Value);
    }

    [Fact]
    public async Task ShouldCreateFeedback()
    {
        var request = new CreateFeedbackDto("Amazing trip!", 5, _user.Id.Value, _place.Id.Value);

        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.IsSuccessStatusCode.Should().BeTrue();

        var dto = await response.ToResponseModel<FeedbackDto>();
        dto.Comment.Should().Be("Amazing trip!");
        dto.Rating.Should().Be(5);

        var dbEntity = (await Context.Feedbacks.ToListAsync())
            .First(f => f.Id.Value == dto.Id);
        dbEntity.Comment.Should().Be("Amazing trip!");
        dbEntity.Rating.Should().Be(5);
    }

    [Fact]
    public async Task ShouldUpdateFeedback()
    {
        var update = new UpdateFeedbackDto(_firstFeedback.Id.Value, "Updated comment", 4);

        var response = await Client.PutAsJsonAsync(BaseRoute, update);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dto = await response.ToResponseModel<FeedbackDto>();
        dto.Comment.Should().Be("Updated comment");
        dto.Rating.Should().Be(4);

        var dbEntity = await Context.Feedbacks.FindAsync(_firstFeedback.Id);
        dbEntity!.Comment.Should().Be("Updated comment");
        dbEntity.Rating.Should().Be(4);
        dbEntity.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldDeleteFeedback()
    {
        var response = await Client.DeleteAsync($"{BaseRoute}/{_firstFeedback.Id.Value}");
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);

        var exists = await Context.Feedbacks.AnyAsync(f => f.Id == _firstFeedback.Id);
        exists.Should().BeFalse();
    }

    // -------------------------------
    // ❌ UNSUCCESSFUL TESTS
    // -------------------------------

    [Fact]
    public async Task ShouldReturnNotFound_WhenFeedbackDoesNotExist()
    {
        var response = await Client.GetAsync($"{BaseRoute}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldNotCreateFeedback_WhenPlaceDoesNotExist()
    {
        var request = new CreateFeedbackDto("Nice", 5, _user.Id.Value, Guid.NewGuid());
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldNotCreateFeedback_WhenUserDoesNotExist()
    {
        var request = new CreateFeedbackDto("Cool", 4, Guid.NewGuid(), _place.Id.Value);
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task ShouldNotCreateFeedback_WhenCommentIsInvalid(string? comment)
    {
        var request = new CreateFeedbackDto(comment!, 3, _user.Id.Value, _place.Id.Value);
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(6)]
    public async Task ShouldNotCreateFeedback_WhenRatingIsInvalid(int rating)
    {
        var request = new CreateFeedbackDto("Bad", rating, _user.Id.Value, _place.Id.Value);
        var response = await Client.PostAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task ShouldNotUpdateFeedback_WhenFeedbackDoesNotExist()
    {
        var request = new UpdateFeedbackDto(Guid.NewGuid(), "Nonexistent", 2);
        var response = await Client.PutAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task ShouldNotUpdateFeedback_WhenCommentIsInvalid(string? comment)
    {
        var request = new UpdateFeedbackDto(_firstFeedback.Id.Value, comment!, 3);
        var response = await Client.PutAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(10)]
    public async Task ShouldNotUpdateFeedback_WhenRatingIsInvalid(int rating)
    {
        var request = new UpdateFeedbackDto(_firstFeedback.Id.Value, "Invalid rating", rating);
        var response = await Client.PutAsJsonAsync(BaseRoute, request);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task ShouldNotDeleteFeedback_WhenFeedbackDoesNotExist()
    {
        var response = await Client.DeleteAsync($"{BaseRoute}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // -------------------------------
    // ⚙️ TEST SETUP / CLEANUP
    // -------------------------------

    public async Task InitializeAsync()
    {
        Context.Countries.RemoveRange(Context.Countries);
        Context.Cities.RemoveRange(Context.Cities);
        Context.Roles.RemoveRange(Context.Roles);
        Context.Users.RemoveRange(Context.Users);
        Context.Places.RemoveRange(Context.Places);
        Context.Feedbacks.RemoveRange(Context.Feedbacks);
        await Context.SaveChangesAsync();

        // Потім створюй свої тестові сутності
        var country = CountriesData.FirstTestCountry();
        var city = CitiesData.FirstTestCity(country);
        var role = RolesData.FirstTestRole();

        _user = UsersData.FirstTestUser(role, city);
        _place = PlacesData.FirstTestPlace();

        _firstFeedback = Feedback.New("Great experience!", 5, _user.Id, _place.Id);
        _secondFeedback = Feedback.New("Could be better", 3, _user.Id, _place.Id);

        await Context.Countries.AddAsync(country);
        await Context.Cities.AddAsync(city);
        await Context.Roles.AddAsync(role);
        await Context.Users.AddAsync(_user);
        await Context.Places.AddAsync(_place);
        await Context.Feedbacks.AddRangeAsync(_firstFeedback, _secondFeedback);

        await SaveChangesAsync();
    }


    public async Task DisposeAsync()
    {
        Context.Feedbacks.RemoveRange(Context.Feedbacks);
        Context.Places.RemoveRange(Context.Places);
        Context.Users.RemoveRange(Context.Users);
        await SaveChangesAsync();
    }
}
