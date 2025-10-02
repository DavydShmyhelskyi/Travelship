using Domain.Places;

namespace Domain.PlacePhotos
{
    public class PlacePhoto
    {
        public Guid Id { get; }
        public byte[] Photo { get; private set; }
        public string Description { get; private set; }
        public bool IsShown { get; private set; }
        //Place
        public Guid PlaceId { get; private set; }
        public Place? Place { get; private set; }

        private PlacePhoto(Guid id, byte[] photo, string description, Guid placeId)
        {
            Id = id;
            Photo = photo;
            Description = description;
            PlaceId = placeId;
            IsShown = true;
        }
        public static PlacePhoto New(byte[] photo, string description, Guid placeId)
        {
            return new PlacePhoto(Guid.NewGuid(), photo, description, placeId);
        }
        public void Update(byte[] photo, string description)
        {
            Photo = photo;
            Description = description;
        }
        public void ChangeVisibility(bool isShown)
        {
            IsShown = isShown;
        }

    }
}
