using Domain.Followers;
using Tests.Data.Users;
using Tests.Data.Cities;
using Tests.Data.Countries;
using Tests.Data.Roles;
using Domain.Cities;
using Domain.Countries;
using Domain.Roles;

namespace Tests.Data.Followers
{
    public static class FollowersData
    {
        public static Follower FirstTestFollower()
        {
            var country = CountriesData.FirstTestCountry();
            var city = CitiesData.FirstTestCity(country);
            var role = RolesData.FirstTestRole();

            var followerUser = UsersData.FirstTestUser(role, city);

            var secondCountry = CountriesData.SecondTestCountry();
            var secondCity = CitiesData.SecondTestCity(secondCountry);
            var secondRole = RolesData.SecondTestRole();

            var followedUser = UsersData.SecondTestUser(secondRole, secondCity);

            return Follower.New(followerUser.Id, followedUser.Id);
        }

        public static Follower SecondTestFollower()
        {
            var country = CountriesData.FirstTestCountry();
            var city = CitiesData.FirstTestCity(country);
            var role = RolesData.FirstTestRole();

            var firstUser = UsersData.FirstTestUser(role, city);

            var secondCountry = CountriesData.SecondTestCountry();
            var secondCity = CitiesData.SecondTestCity(secondCountry);
            var secondRole = RolesData.SecondTestRole();

            var secondUser = UsersData.SecondTestUser(secondRole, secondCity);

            return Follower.New(secondUser.Id, firstUser.Id);
        }
    }
}
