using Domain.Cities;

namespace Domain.Countries
{
    public class Country
    {
        public CountryId Id { get; }
        public string Title { get; private set; }

        public ICollection<City> Cities { get; private set; } = new List<City>();

        private Country(CountryId id, string title)
        {
            Id = id;
            Title = title;
        }

        public static Country New(string title)
            => new(CountryId.New(), title);

        public void ChangeTitle(string title)
            => Title = title;
    }
}
