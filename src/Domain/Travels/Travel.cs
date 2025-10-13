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

        public IEnumerable<User> Members { get; private set; } = new List<User>();

        private Travel(TravelId id, string title, DateTime startDate, DateTime endDate, string description, byte[]? image, bool isDone, UserId userId)
        {
            if (startDate > endDate)
                throw new ArgumentException("Start date cannot be later than end date.");

            Id = id;
            Title = title;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            Image = image;
            IsDone = isDone;
            UserId = userId;
        }

        public static Travel New(string title, DateTime startDate, DateTime endDate, string description, byte[]? image, bool isDone, UserId userId)
            => new(TravelId.New(), title, startDate, endDate, description, image, isDone, userId);

        public void Update(string title, DateTime startDate, DateTime endDate, string description, byte[]? image, bool isDone)
        {
            if (startDate > endDate)
                throw new ArgumentException("Start date cannot be later than end date.");

            Title = title;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            Image = image;
            IsDone = isDone;
        }
    }
}
