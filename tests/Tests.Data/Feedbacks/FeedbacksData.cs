using Domain.Feedbacks;
using Tests.Data.Places;
using Tests.Data.Users;
using Tests.Data.Cities;
using Tests.Data.Countries;
using Tests.Data.Roles;
using Domain.Cities;
using Domain.Countries;
using Domain.Roles;

namespace Tests.Data.Feedbacks
{
    public static class FeedbacksData
    {
        public static Feedback FirstTestFeedback()
        {
            var country = CountriesData.FirstTestCountry();
            var city = CitiesData.FirstTestCity(country);
            var role = RolesData.FirstTestRole();

            var user = UsersData.FirstTestUser(role, city);
            var place = PlacesData.FirstTestPlace();

            return Feedback.New(
                "Great place to visit!",
                5,
                user.Id,
                place.Id);
        }

        public static Feedback SecondTestFeedback()
        {
            var country = CountriesData.SecondTestCountry();
            var city = CitiesData.SecondTestCity(country);
            var role = RolesData.SecondTestRole();

            var user = UsersData.SecondTestUser(role, city);
            var place = PlacesData.SecondTestPlace();

            return Feedback.New(
                "Not worth the hype.",
                2,
                user.Id,
                place.Id);
        }
    }
}
