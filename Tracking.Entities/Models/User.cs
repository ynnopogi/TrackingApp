using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Tracking.Entities.Models
{
    public class User : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string UserIdentityId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        [JsonIgnore]
        public string Password { get; set; }

        [ForeignKey("UserIdentityId")]
        public virtual UserIdentity UserIdentity { get; set; }
    }
}