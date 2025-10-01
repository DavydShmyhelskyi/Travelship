using Domain.Places;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Feedbacks
{
    public class Feedback
    {
        public Guid Id { get; }
        public string Comment { get; private set; }
        public int Rating { get; private set; } 
        public DateTime Date { get; private set; }
        //User
        public User User { get; private set; }
        public Guid UserId { get; private set; }
        //Place
        public Place Place { get; private set; }
        public Guid PlaceId { get; private set; }

        private Feedback(Guid id, string comment, int rating, DateTime date, Guid userId, Guid placeId)
        {
            Id = id;
            Comment = comment;
            Rating = rating;
            Date = date;
            UserId = userId;
            PlaceId = placeId;
        }
        public static Feedback New(string comment, int rating, Guid userId, Guid placeId)
        {
            return new Feedback(Guid.NewGuid(), comment, rating, DateTime.UtcNow, userId, placeId);
        }
        public void Update(string comment, int rating)
        {
            Comment = comment;
            Rating = rating;
            Date = DateTime.UtcNow;
        }
    }
}
