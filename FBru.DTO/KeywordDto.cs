using System.Collections.Generic;

namespace FBru.DTO
{
    public class KeywordDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class KeywordWithDishesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<DishWithCategoryAndRestaurantDto> Dishes { get; set; }
    }
}
