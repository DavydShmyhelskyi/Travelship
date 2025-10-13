namespace Domain.Cities
{
    public record CityId(Guid Value)
    {
        public static CityId Empty() => new(Guid.Empty);
        public static CityId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
