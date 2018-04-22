using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FBru.DTO;

namespace FBru.WebAdmin.Models
{
    public class DishModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Restaurant")]
        public int RestaurantId { get; set; }

        [Display(Name = "Keywords")]
        public IEnumerable<int> KeywordIds { get; set; }

        public IEnumerable<KeywordDto> Keywords { get; set; }
        public IEnumerable<ImageDto> Images { get; set; }
    }
}