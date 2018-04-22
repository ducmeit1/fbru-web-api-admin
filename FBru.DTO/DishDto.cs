using System.Collections.Generic;

namespace FBru.DTO
{
    public class DishSimpleDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DishWithPrice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
    }

    public class DishWithCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public CategoryDto Category { get; set; }
    }

    public class DishWithRestaurantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public RestaurantDto Restaurant { get; set; }
    }

    public class DishWithCategoryAndRestaurantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public CategoryDto Category { get; set; }
        public RestaurantDto Restaurant { get; set; }
    }

    public class DishDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public CategoryDto Category { get; set; }
        public RestaurantDto Restaurant { get; set; }
        public IEnumerable<ImageDto> Images { get; set; }
        public IEnumerable<KeywordDto> Keywords { get; set; }
    }
}
