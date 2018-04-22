using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBru.Repository.Entities
{
    public partial class Dish
    {
        public Dish()
        {
            Images = new HashSet<Image>();
            Keywords = new HashSet<Keyword>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(255)]
        public string Name { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Restaurant")]
        public int RestaurantId { get; set; }
        public virtual Category Category { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Keyword> Keywords { get; set; }
    }
}