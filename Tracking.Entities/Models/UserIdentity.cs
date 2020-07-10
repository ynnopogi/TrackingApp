using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tracking.Entities.Models
{
    [Table("AspNetUsers")]
    public class UserIdentity:IEntity
    {
        public UserIdentity() => Users = new HashSet<User>();

        [Key]
        public string Id { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}