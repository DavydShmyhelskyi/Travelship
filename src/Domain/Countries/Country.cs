using Domain.Cities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Countries
{
    public class Country
    {
        public Guid Id { get; }
        public string Title { get; private set; }
        IEnumerable<City> Cities { get; set; }
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
