using Domain.Travels;

namespace Domain.Users
{
    public class UserTravel
    {
        public UserId UserId { get; init; }
        public User? user { get; init; }
        public TravelId TravelId { get; init; }
        public Travel? travel { get; init; }

        private UserTravel(UserId userId, TravelId travelId) => (UserId, TravelId) = (userId, travelId);
        public static UserTravel New(UserId userId, TravelId travelId) => new(userId, travelId);
    }
}
