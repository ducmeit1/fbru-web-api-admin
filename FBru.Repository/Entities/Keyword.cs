using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBru.Repository.Entities
{
    public partial class Keyword
    {
        public Keyword()
        {
            Dishes = new HashSet<Dish>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        [Column(TypeName = "nvarchar")]
        public string Name { get; set; }
        public virtual ICollection<Dish> Dishes { get; set; }
    }
}