using Domain.Cities;
using Domain.Roles;
using Domain.Users;

namespace Tests.Data.Users
{
    public static class UsersData
    {
       // public static User New(string nickName, byte[]? avatar, string email, string password, RoleId roleId, CityId? cityId)
        public static User FirstTestUser() =>
            User.New(
                "FirstTestUser",
                null,
                "firsttestuser@gmail.com",
                "FirstTestUserPasswordHash1!",
                Roles.RolesData.FirstTestRole().Id,
                Cities.CitiesData.FirstTestCity().Id);

        public static User SecondTestUser() =>
            User.New(
                "SecondTestUser",
                null,
                "secondtestuser@gmail.com",
                "SecondTestUserPasswordHash2!",
                Roles.RolesData.SecondTestRole().Id,
                Cities.CitiesData.SecondTestCity().Id);
    }
}
