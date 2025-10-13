namespace Domain.Countries
{
    public record CountryId(Guid Value)
    {
        public static CountryId Empty() => new(Guid.Empty);
        public static CountryId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
