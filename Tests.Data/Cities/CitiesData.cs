using Domain.Cities;
using Tests.Data.Countries;

namespace Tests.Data.Cities
{
    public static class CitiesData
    {
        public static City FirstTestCity() =>
            City.New("First Test City", CountriesData.FirstTestCountry().Id);

        public static City SecondTestCity() =>
            City.New("Second Test City", CountriesData.SecondTestCountry().Id);
    }
}