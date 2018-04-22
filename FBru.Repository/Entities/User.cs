using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBru.Repository.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [MaxLength(255)]
        [Required]
        public string Password { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [DataType(DataType.PhoneNumber)]
        [MaxLength(255)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Group")]
        public int GroupId { get; set; }
        public virtual UserGroup UserGroup { get; set; }
    }
}
