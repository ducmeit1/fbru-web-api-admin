using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FBru.Repository.Entities
{
    public class UserGroup
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
