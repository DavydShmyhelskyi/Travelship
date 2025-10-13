using Domain.Countries;
using Domain.Users;

namespace Domain.Cities
{
    public class City
    {
        public CityId Id { get; }
        public string Title { get; private set; }

        public CountryId CountryId { get; private set; }
        public Country? Country { get; private set; }

        public IEnumerable<User> Users { get; private set; } = new List<User>();

        private City(CityId id, string title, CountryId countryId)
        {
            Id = id;
            Title = title;
            CountryId = countryId;
        }

        public static City New(string title, CountryId countryId)
            => new(CityId.New(), title, countryId);

        public void ChangeTitle(string title)
            => Title = title;
    }
}
