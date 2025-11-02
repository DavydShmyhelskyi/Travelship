using Domain.PlacePhotos;
using Tests.Data.Places;

namespace Tests.Data.PlacePhotos
{
    public static class PlacePhotosData
    {
        public static PlacePhoto FirstTestPhoto() =>
            PlacePhoto.New(
                new byte[] { 0x42, 0x24, 0x66 }, // умовне зображення
                "Test Photo 1",
                PlacesData.FirstTestPlace().Id
            );

        public static PlacePhoto SecondTestPhoto() =>
            PlacePhoto.New(
                new byte[] { 0x99, 0xAA, 0x77 },
                "Test Photo 2",
                PlacesData.SecondTestPlace().Id
            );
    }
}
