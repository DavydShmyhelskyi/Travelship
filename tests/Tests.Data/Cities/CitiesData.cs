using Domain.Cities;
using Domain.Countries;

namespace Tests.Data.Cities
{
    public static class CitiesData
    {
        public static City FirstTestCity(Country country) =>
            City.New("Paris", country.Id);

        public static City SecondTestCity(Country country) =>
            City.New("New York", country.Id);
    }
}
