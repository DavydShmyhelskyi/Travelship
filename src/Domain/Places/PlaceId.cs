namespace Domain.Places
{
    public record PlaceId(Guid Value)
    {
        public static PlaceId Empty() => new(Guid.Empty);
        public static PlaceId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
