namespace Domain.Travels
{
    public record TravelId(Guid Value)
    {
        public static TravelId Empty() => new(Guid.Empty);
        public static TravelId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
