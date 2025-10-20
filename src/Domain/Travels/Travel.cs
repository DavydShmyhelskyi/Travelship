using Domain.Places;
using Domain.Users;

namespace Domain.Travels
{
    public class Travel
    {
        public TravelId Id { get; }
        public string Title { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public string Description { get; private set; }
        public byte[]? Image { get; private set; }
        public bool IsDone { get; private set; }
        public UserId UserId { get; private set; }
        public User? User { get; private set; }

        public ICollection<UserTravel>? Members { get; private set; } = new List<UserTravel>();
        public ICollection<TravelPlace> Places { get; private set; } = new List<TravelPlace>();

        private Travel(TravelId id, string title, DateTime startDate, DateTime endDate, string description, byte[]? image, UserId userId, ICollection<UserTravel> members, ICollection<TravelPlace> places)
        {
            if (startDate > endDate)
                throw new ArgumentException("Start date cannot be later than end date.");

            Id = id;
            Title = title;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            Image = image;
            IsDone = false;
            UserId = userId; 
            Members = members;
            Places = places;
        }

        public static Travel New(TravelId id, string title, DateTime startDate, DateTime endDate, string description, byte[]? image, UserId userId, ICollection<UserTravel> members,  ICollection<TravelPlace> places)
            => new(id, title, startDate, endDate, description, image, userId, members, places);

        public void Update(string title, DateTime startDate, DateTime endDate, string description, byte[]? image, bool isDone, ICollection<UserTravel> members, ICollection<TravelPlace> places)
        {
            if (startDate > endDate)
                throw new ArgumentException("Start date cannot be later than end date.");

            Title = title;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            Image = image;
            IsDone = isDone;
            Members = members;
            Places = places;
        }

    }
}
