using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBru.Repository.Entities
{
    public partial class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(255)]
        public string Name { get; set; }
        public string Url { get; set; }
        [Required]
        [Display(Name = "Display")]
        public bool IsDisplay { get; set; }
        [Required]
        [Display(Name = "Dish")]
        public int DishId { get; set; }
        public virtual Dish Dish { get; set; }
    }
}