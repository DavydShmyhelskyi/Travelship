using Domain.Places;

namespace Domain.PlacePhotos
{
    public class PlacePhoto
    {
        public PlacePhotoId Id { get; }
        public byte[] Photo { get; private set; }
        public string Description { get; private set; }
        public bool IsShown { get; private set; }

        public PlaceId PlaceId { get; private set; }
        public Place? Place { get; private set; }

        private PlacePhoto(PlacePhotoId id, byte[] photo, string description, PlaceId placeId)
        {
            Id = id;
            Photo = photo;
            Description = description;
            PlaceId = placeId;
            IsShown = true;
        }

        public static PlacePhoto New(byte[] photo, string description, PlaceId placeId)
            => new(PlacePhotoId.New(), photo, description, placeId);

        public void Update(byte[] photo, string description)
        {
            Photo = photo;
            Description = description;
        }

        public void ChangeVisibility(bool isShown)
            => IsShown = isShown;
    }
}
