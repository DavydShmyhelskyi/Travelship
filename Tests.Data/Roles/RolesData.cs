using Domain.Roles;

namespace Tests.Data.Roles
{
    public static class RolesData
    {
        public static Role FirstTestRole() => Role.New(RoleId.New(),"First Test Role");
        public static Role SecondTestRole() => Role.New(RoleId.New(), "Second Test Role");
    }
}
