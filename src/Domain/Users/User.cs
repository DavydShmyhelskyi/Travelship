using Domain.Cities;
using Domain.Roles;
using Domain.Travels;

namespace Domain.Users
{
    public class User
    {
        public UserId Id { get; }
        public string NickName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreatedAt { get; }

        public RoleId RoleId { get; private set; }
        public Role? Role { get; private set; }

        public CityId? CityId { get; private set; }
        public City? City { get; private set; }

        public IEnumerable<Travel> Travels { get; private set; } = new List<Travel>();
        public IEnumerable<Travel> MemberTravels { get; private set; } = new List<Travel>();

        private User(UserId id, string nickName, string email, string passwordHash, RoleId roleId, CityId? cityId)
        {
            Id = id;
            NickName = nickName;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
            RoleId = roleId;
            CityId = cityId;
        }

        public static User New(string nickName, string email, string password, RoleId roleId, CityId? cityId)
            => new(UserId.New(), nickName, email, BCrypt.Net.BCrypt.HashPassword(password), roleId, cityId);

        public void Update(string nickName, string email, RoleId roleId, CityId? cityId)
        {
            NickName = nickName;
            Email = email;
            RoleId = roleId;
            CityId = cityId;
        }

        public void ChangePassword(string newPassword)
            => PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

        public bool VerifyPassword(string password)
            => BCrypt.Net.BCrypt.Verify(password, PasswordHash);
    }
}
