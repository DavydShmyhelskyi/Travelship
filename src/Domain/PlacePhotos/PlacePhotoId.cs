namespace Domain.PlacePhotos
{
    public record PlacePhotoId(Guid Value)
    {
        public static PlacePhotoId Empty() => new(Guid.Empty);
        public static PlacePhotoId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
