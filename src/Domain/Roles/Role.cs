using Domain.Users;

namespace Domain.Roles
{
    public class Role
    {
        public Guid Id { get; }
        public string Title { get; private set; }
        public IEnumerable<User> Users { get; set; } = new List<User>();

        private Role(Guid id, string title)
        {
            Id = id;
            Title = title;
        }
        public static Role New(string title)
        {
            return new Role(Guid.NewGuid(), title);
        }
        public void ChangeTitle(string title)
        {
            Title = title;
        }
    }
}
