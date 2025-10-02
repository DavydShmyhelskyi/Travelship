using Domain.Users;
using System.Data;

namespace Domain.Travels
{
    public class Travel
    {
        public Guid Id { get; }
        public string Title { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public string Description { get; private set; }
        public byte[]? Image { get; private set; }
        public bool IsDone { get; private set; }
        //user
        public Guid UserId { get; private set; }
        public User? user { get; private set; }
        // members
        public IEnumerable<User> Members { get; set; } = new List<User>();

        private Travel(Guid id, string title, DateTime startDate, DateTime endDate, string description, byte[]? image, bool isDone, Guid userId)
        {
            Id = id;
            Title = title;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            Image = image;
            IsDone = isDone;
            UserId = userId;
        }
        public static Travel New(string title, DateTime startDate, DateTime endDate, string description, byte[]? image, bool isDone, Guid userId)
        {
            return new Travel(Guid.NewGuid(), title, startDate, endDate, description, image, isDone, userId);
        }
        public void Update(string title, DateTime startDate, DateTime endDate, string description, byte[]? image, bool isDone)
        {
            if (startDate > endDate)
            {
                throw new NoNullAllowedException("Start date cannot be later than end date.");
            }
            else { 
                Title = title;
                StartDate = startDate;
                EndDate = endDate;
                Description = description;
                Image = image;
                IsDone = isDone;
            }
        }
    }
}
