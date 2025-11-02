using Domain.Followers;
using Domain.Users;

namespace Tests.Data.Folowers
{
    public static class FolowersData
    {
        public static Follower NewFollower(
            UserId followerId,
            UserId followeeId)
            => Follower.New(
                followerId,
                followeeId);
    }
}
