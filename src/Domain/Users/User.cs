using Domain.Cities;
using Domain.Roles;
using Domain.Travels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public class User
    {
        public Guid Id { get; }
        public string NickName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreatedAt { get; }
        //Role
        public Guid RoleId { get; private set; }
        public Role Role { get; private set; }
        //City
        public Guid? CityId { get; private set; }
        public City? City { get; private set; }
        //Travels
        public IEnumerable<Travel> Travels { get; set; }
        //MemberTravels
        public IEnumerable<Travel> MemberTravels { get; set; }

        private User(Guid id, string nickName, string email, string password, Guid roleId, Guid? cityId)
        {
            Id = id;
            NickName = nickName;
            Email = email;
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            CreatedAt = DateTime.UtcNow;
            RoleId = roleId;
            CityId = cityId;
        }
        public static User New(string nickName, string email, string password, Guid roleId, Guid? cityId)
        {
            return new User(Guid.NewGuid(), nickName, email, password, roleId, cityId);
        }
        public void Update(string nickName, string email, Guid roleId, Guid? cityId)
        {
            NickName = nickName;
            Email = email;            
            RoleId = roleId;
            CityId = cityId;
        }
        public void ChangePassword(string newPassword)
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        }
        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }
    }
}
