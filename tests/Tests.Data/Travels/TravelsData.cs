using Domain.Travels;
using Domain.Users;
using Domain.Places;
using Domain.Cities;
using Domain.Countries;
using Domain.Roles;
using Tests.Data.Users;
using Tests.Data.Places;
using Tests.Data.Cities;
using Tests.Data.Countries;
using Tests.Data.Roles;

namespace Tests.Data.Travels
{
    public static class TravelsData
    {
        public static Travel FirstTestTravel()
        {
            var country = CountriesData.FirstTestCountry();
            var city = CitiesData.FirstTestCity(country);
            var role = RolesData.FirstTestRole();

            var firstUser = UsersData.FirstTestUser(role, city);
            var secondUser = UsersData.SecondTestUser(role, city);

            var firstPlace = PlacesData.FirstTestPlace();
            var secondPlace = PlacesData.SecondTestPlace();

            var travelId = TravelId.New();

            var members = new List<UserTravel>
            {
                CreateTestUserTravel(firstUser.Id, travelId),
                CreateTestUserTravel(secondUser.Id, travelId)
            };

            var places = new List<TravelPlace>
            {
                CreateTestTravelPlace(travelId, firstPlace.Id),
                CreateTestTravelPlace(travelId, secondPlace.Id)
            };

            return Travel.New(
                id: travelId,
                title: "Summer Vacation",
                startDate: new DateTime(2025, 7, 1),
                endDate: new DateTime(2025, 7, 15),
                description: "A relaxing summer vacation by the beach.",
                image: null,
                userId: firstUser.Id,
                members: members,
                places: places
            );
        }

        public static Travel SecondTestTravel()
        {
            var country = CountriesData.SecondTestCountry();
            var city = CitiesData.SecondTestCity(country);
            var role = RolesData.SecondTestRole();

            var firstUser = UsersData.FirstTestUser(role, city);
            var secondUser = UsersData.SecondTestUser(role, city);

            var firstPlace = PlacesData.FirstTestPlace();
            var secondPlace = PlacesData.SecondTestPlace();

            var travelId = TravelId.New();

            var members = new List<UserTravel>
            {
                CreateTestUserTravel(secondUser.Id, travelId),
                CreateTestUserTravel(firstUser.Id, travelId)
            };

            var places = new List<TravelPlace>
            {
                CreateTestTravelPlace(travelId, secondPlace.Id),
                CreateTestTravelPlace(travelId, firstPlace.Id)
            };

            return Travel.New(
                id: travelId,
                title: "Winter Getaway",
                startDate: new DateTime(2025, 12, 20),
                endDate: new DateTime(2026, 1, 5),
                description: "A cozy winter getaway in the mountains.",
                image: null,
                userId: secondUser.Id,
                members: members,
                places: places
            );
        }

        public static UserTravel CreateTestUserTravel(UserId userId, TravelId travelId)
            => UserTravel.New(userId, travelId);

        public static TravelPlace CreateTestTravelPlace(TravelId travelId, PlaceId placeId)
            => TravelPlace.New(travelId, placeId);
    }
}
