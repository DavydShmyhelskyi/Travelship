using Domain.PlacePhotos;

namespace Domain.Places
{
    public class Place
    {
        public PlaceId Id { get; }
        public string Title { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public ICollection<PlacePhoto>? PlacePhotos { get; private set; } = new List<PlacePhoto>();
        public ICollection<TravelPlace> Travels { get; private set; } = new List<TravelPlace>();


        private Place(PlaceId id, string title, double latitude, double longitude)
        {
            Id = id;
            Title = title;
            Latitude = latitude;
            Longitude = longitude;
        }

        public static Place New(string title, double latitude, double longitude)
            => new(PlaceId.New(), title, latitude, longitude);

        public void Update(string title, double latitude, double longitude)
        {
            Title = title;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
