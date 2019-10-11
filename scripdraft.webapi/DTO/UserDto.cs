using ScripDraft.Data.Entities;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ScripDraft.WebApi.DTO
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        internal static User CreateEntity(UserDto userDto) =>
            new User()
            {
                Id = userDto.Id.ToString().Equals("00000000-0000-0000-0000-000000000000") ? Guid.NewGuid() : userDto.Id,
                Name = userDto.Name,
                Username = userDto.Username,
                Email = userDto.Email
            };
    }
}