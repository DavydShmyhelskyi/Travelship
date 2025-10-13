using Domain.Users;

namespace Domain.Followers
{
    public class Follower
    {
        public DateTime Date { get; }

        // User who follows
        public UserId FollowerUserId { get; }
        public User? FollowerUser { get; }

        // User being followed
        public UserId FollowedUserId { get; }
        public User? FollowedUser { get; }

        private Follower(DateTime date, UserId followerUserId, UserId followedUserId)
        {
            Date = date;
            FollowerUserId = followerUserId;
            FollowedUserId = followedUserId;
        }

        public static Follower New(UserId followerUserId, UserId followedUserId)
        {
            return new Follower(DateTime.UtcNow, followerUserId, followedUserId);
        }
    }
}
