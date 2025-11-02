using Domain.Feedbacks;
using Tests.Data.Places;
using Tests.Data.Users;

namespace Tests.Data.Feedbacks
{
    public static class FeedbacksData
    {
        public static Feedback FirstTestFeedback()
            => Feedback.New(
                "Great place to visit!",
                5,
                UsersData.FirstTestUser().Id,
                PlacesData.FirstTestPlace().Id);

        public static Feedback SecondTestFeedback()
            => Feedback.New(
                "Not worth the hype.",
                2,
                UsersData.SecondTestUser().Id,
                PlacesData.SecondTestPlace().Id);
    }
}
