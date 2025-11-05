using Domain.Users;
using Domain.Cities;
using Domain.Roles;

namespace Tests.Data.Users
{
    public static class UsersData
    {
        public static User FirstTestUser(Role role, City city) =>
            User.New(
                "FirstTestUser",
                null,
                "firsttestuser@gmail.com",
                "FirstTestUserPasswordHash1!",
                role.Id,
                city.Id);

        public static User SecondTestUser(Role role, City city) =>
            User.New(
                "SecondTestUser",
                null,
                "secondtestuser@gmail.com",
                "SecondTestUserPasswordHash2!",
                role.Id,
                city.Id);
    }
}
