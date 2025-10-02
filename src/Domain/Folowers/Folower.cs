using Domain.Users;

namespace Domain.Folowers
{
    public class Folower
    {
        public DateTime Date { get; }

        // User who follows
        public Guid FollowerId { get; }
        public User? Follower { get; }

        // User being followed
        public Guid FollowedId { get; }
        public User? Followed { get; }

        private Folower(DateTime date, Guid followerId, Guid followedId)
        {
            Date = date;
            FollowerId = followerId;
            FollowedId = followedId;
        }
        public static Folower New(Guid followerId, Guid followedId)
        {
            return new Folower(DateTime.UtcNow, followerId, followedId);
        }
    }
}
