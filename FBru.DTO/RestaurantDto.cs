using System;
using System.Collections.Generic;

namespace FBru.DTO
{
    public class RestaurantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class RestaurantWithDishesHaveCategoryDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public bool IsHalal { get; set; }
        public IEnumerable<DishWithCategoryDto> Dishes { get; set; }
        public int NumberOfDishes { get; set; }
    }

    public class RestaurantWithDishesHaveRestaurantDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public bool IsHalal { get; set; }
        public IEnumerable<DishWithRestaurantDto> Dishes { get; set; }
        public int NumberOfDishes { get; set; }
    }

    public class RestaurantWithNumberOfDishes
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public bool IsHalal { get; set; }
        public int NumberOfDishes { get; set; }
    }
}
