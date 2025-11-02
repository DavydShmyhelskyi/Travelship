using Domain.Countries;

namespace Tests.Data.Countries
{
    public static class CountriesData
    {
        public static Country FirstTestCountry() => Country.New("First Test Country");
        public static Country SecondTestCountry() => Country.New("Second Test Country");

    }
}
