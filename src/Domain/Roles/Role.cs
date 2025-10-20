using Domain.Users;

namespace Domain.Roles
{
    public class Role
    {
        public RoleId Id { get; }
        public string Title { get; private set; }
        public ICollection<User> Users { get; set; } = new List<User>();

        private Role(RoleId id, string title)
        {
            Id = id;
            Title = title;
        }
        public static Role New(RoleId id, string title)
        {
            return new Role(id, title);
        }
        public void ChangeTitle(string title)
        {
            Title = title;
        }
    }
}
