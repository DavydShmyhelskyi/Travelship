using Domain.Travels;
using Domain.Users;
using Domain.Places;
using Tests.Data.Users;
using Tests.Data.Places;

namespace Tests.Data.Travels
{
    public static class TravelsData
    {
        public static Travel FirstTestTravel()
        {
            var travelId = TravelId.New();
            var firstUserId = UsersData.FirstTestUser().Id;
            var secondUserId = UsersData.SecondTestUser().Id;
            var firstPlaceId = PlacesData.FirstTestPlace().Id;
            var secondPlaceId = PlacesData.SecondTestPlace().Id;

            var members = new List<UserTravel>
            {
                CreateTestUserTravel(firstUserId, travelId),
                CreateTestUserTravel(secondUserId, travelId)
            };

            var places = new List<TravelPlace>
            {
                CreateTestTravelPlace(travelId, firstPlaceId),
                CreateTestTravelPlace(travelId, secondPlaceId)
            };

            return Travel.New(
                id: travelId,
                title: "Summer Vacation",
                startDate: new DateTime(2025, 7, 1),
                endDate: new DateTime(2025, 7, 15),
                description: "A relaxing summer vacation by the beach.",
                image: null,
                userId: firstUserId,
                members: members,
                places: places
            );
        }

        public static Travel SecondTestTravel()
        {
            var travelId = TravelId.New();
            var firstUserId = UsersData.FirstTestUser().Id;
            var secondUserId = UsersData.SecondTestUser().Id;
            var firstPlaceId = PlacesData.FirstTestPlace().Id;
            var secondPlaceId = PlacesData.SecondTestPlace().Id;
            var members = new List<UserTravel>
            {
                CreateTestUserTravel(secondUserId, travelId),
                CreateTestUserTravel(firstUserId, travelId)
            };
            var places = new List<TravelPlace>
            {
                CreateTestTravelPlace(travelId, secondPlaceId),
                CreateTestTravelPlace(travelId, firstPlaceId)
            };
            return Travel.New(
                id: travelId,
                title: "Winter Getaway",
                startDate: new DateTime(2025, 12, 20),
                endDate: new DateTime(2026, 1, 5),
                description: "A cozy winter getaway in the mountains.",
                image: null,
                userId: secondUserId,
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
