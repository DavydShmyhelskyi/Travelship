using Domain.PlacePhotos;
using Domain.Places;

namespace Tests.Data.PlacePhotos
{
    public static class PlacePhotosData
    {
        public static PlacePhoto FirstTestPhoto(Place place) =>
            PlacePhoto.New(
                new byte[] { 0x42, 0x24, 0x66 }, // умовне зображення
                "Test Photo 1",
                place.Id
            );

        public static PlacePhoto SecondTestPhoto(Place place) =>
            PlacePhoto.New(
                new byte[] { 0x99, 0xAA, 0x77 },
                "Test Photo 2",
                place.Id
            );
    }
}
