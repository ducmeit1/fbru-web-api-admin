using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBru.Repository.Entities
{
    public class Advertisement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int Order { get; set; }
        [Column(TypeName = "nvarchar")]
        [MaxLength(255)]
        public string Title { get; set; }
        [Column(TypeName = "nvarchar")]
        [MaxLength(255)]
        [Display(Name = "Sub Title")]
        public string SubTitle { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }
        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }
        [Display(Name = "Url")]
        public string ActionUrl { get; set; }
        [Required]
        [Display(Name = "Display")]
        public bool IsDisplay { get; set; }
    }
}