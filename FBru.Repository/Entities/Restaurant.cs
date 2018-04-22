using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBru.Repository.Entities
{
    public partial class Restaurant
    {
        public Restaurant()
        {
            Dishes = new HashSet<Dish>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }
        [Required]
        [MaxLength(255)]
        [Column(TypeName = "nvarchar")]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(255)]
        public string Address { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }
        [MaxLength(255)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Open Time")]
        public TimeSpan OpenTime { get; set; }
        [Required]
        [Display(Name = "Close Time")]
        public TimeSpan CloseTime { get; set; }
        [Required]
        [Display(Name = "Halal Food")]
        public bool IsHalal { get; set; }
        public virtual ICollection<Dish> Dishes { get; set; }
    }
}