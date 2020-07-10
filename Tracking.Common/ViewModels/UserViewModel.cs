using System;
using System.Text.Json.Serialization;
using Tracking.Common.Extensions;
using Tracking.Entities.Models;

namespace Tracking.Common.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public UserViewModel Map(User employee) => new UserViewModel
        {
            Id = employee.Id,
            Name = employee.Name,
            Username = employee.Username,
            Password = CryptoExtensions.Decrypt(employee.Password)
        };
    }
}