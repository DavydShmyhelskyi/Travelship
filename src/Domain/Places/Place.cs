using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.PlacePhotos;

namespace Domain.Places
{
    public class Place
    {
        public Guid Id { get; }
        public string Title { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        //PlacePhotos
        public IEnumerable<PlacePhoto> PlacePhotos { get; set; }

        private Place(Guid id, string title, double latitude, double longitude)
        {
            Id = id;
            Title = title;
            Latitude = latitude;
            Longitude = longitude;
        }
        public static Place New(string title, double latitude, double longitude)
        {
            return new Place(Guid.NewGuid(), title, latitude, longitude);
        }
        public void Update(string title, double latitude, double longitude)
        {
            Title = title;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
