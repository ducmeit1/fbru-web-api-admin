using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBru.Repository.Entities
{
    public partial class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Icon { get; set; }
        public virtual ICollection<Dish> Dishes { get; set; }
    }
}