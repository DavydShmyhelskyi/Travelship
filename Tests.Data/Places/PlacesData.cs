using Domain.Places;

namespace Tests.Data.Places
{
    public static class PlacesData
    {
        public static Place FirstTestPlace() =>
            Place.New(
                "Test Place 1",
                48.8584,
                2.2945
            );

        public static Place SecondTestPlace() =>
            Place.New(
                "Test Place 2",
                40.6892,
                -74.0445
            );
    }
}
