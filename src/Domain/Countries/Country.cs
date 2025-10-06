using Domain.Cities;

namespace Domain.Countries
{
    public class Country
    {
        public Guid Id { get; }
        public string Title { get; private set; }
        public IEnumerable<City> Cities { get; set; } = new List<City>();
        private Country(Guid id, string title)
        {
            Id = id;
            Title = title;
        }
        public static Country New(string title)
        {
            return new Country(Guid.NewGuid(), title);
        }
        public void ChangeTitle(string title)
        {
            Title = title;
        }
    }
}
