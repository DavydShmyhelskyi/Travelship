using Domain.Travels;

namespace Domain.Places
{
    public class TravelPlace
    {
        public TravelId TravelId { get; init; }
        public Travel? travel { get; init; }

        public PlaceId PlaceId { get; init; }
        public Place? place { get; init; }

        private TravelPlace(TravelId travelId, PlaceId placeId ) 
            => (TravelId, PlaceId) = (travelId, placeId);

        public static TravelPlace New(TravelId travelId, PlaceId placeId) 
            => new(travelId, placeId);
    }
}
