using Domain.Places;
using Domain.Users;

namespace Domain.Feedbacks
{
    public class Feedback
    {
        public FeedbackId Id { get; }
        public string Comment { get; private set; }
        public int Rating { get; private set; }
        public DateTime Date { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public UserId UserId { get; private set; }
        public User? User { get; private set; }

        public PlaceId PlaceId { get; private set; }
        public Place? Place { get; private set; }

        private Feedback(FeedbackId id, string comment, int rating, DateTime date, UserId userId, PlaceId placeId)
        {
            Id = id;
            Comment = comment;
            Rating = rating;
            Date = date;
            UserId = userId;
            PlaceId = placeId;
        }

        public static Feedback New(string comment, int rating, UserId userId, PlaceId placeId)
            => new(FeedbackId.New(), comment, rating, DateTime.UtcNow, userId, placeId);

        public void Update(string comment, int rating)
        {
            Comment = comment;
            Rating = rating;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
