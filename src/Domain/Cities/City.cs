using Domain.Countries;
using Domain.Users;

namespace Domain.Cities
{
    public class City
    {
        public Guid Id { get; }
        public string Title { get; private set; }
        //Country
        public Guid CountryId { get; private set; }
        public Country? Country { get; private set; }
        //Users
        public IEnumerable<User> Users { get; set; } = new List<User>();

        private City(Guid id, string title, Guid countryId)
        {
            Id = id;
            Title = title;
            CountryId = countryId;
        }
        public static City New(string title, Guid countryId)
        {
            return new City(Guid.NewGuid(), title, countryId);
        }
        public void ChangeTitle(string title)
        {
            Title = title;
        }
    }
}
