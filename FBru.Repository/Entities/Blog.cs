using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBru.Repository.Entities
{
    public partial class Blog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(255)]
        public string Title { get; set; }
        [Required]
        [Column(TypeName = "ntext")]
        public string Description { get; set; }
        [Column(TypeName = "date")]
        [Display(Name = "Publish Date")]
        public DateTime PublishedDate { get; set; }
        [MaxLength(255)]
        [Display(Name = "Author")]
        public string Author { get; set; }
    }
}