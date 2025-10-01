using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Folowers
{
    public class Folower
    {
        public DateOnly Date { get; }

        // User who follows
        public Guid FollowerId { get; }
        public User Follower { get; }

        // User being followed
        public Guid FollowedId { get; }
        public User Followed { get; }

        private Folower(DateOnly date, Guid followerId, Guid followedId)
        {
            Date = date;
            FollowerId = followerId;
            FollowedId = followedId;
        }
        public static Folower New(Guid followerId, Guid followedId)
        {
            return new Folower(DateOnly.FromDateTime(DateTime.UtcNow), followerId, followedId);
        }
    }
}
