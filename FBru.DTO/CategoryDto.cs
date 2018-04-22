using System.Collections.Generic;

namespace FBru.DTO
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CategoryWithIconDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }

    public class CategoryWithDishesHaveRestaurantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public IEnumerable<DishWithRestaurantDto> Dishes { get; set; }
        public int NumberOfDishes { get; set; }
    }
}
