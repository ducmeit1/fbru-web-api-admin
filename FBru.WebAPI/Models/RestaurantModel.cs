using System;
using System.Collections.Generic;

namespace FBru.WebAPI.Models
{
    public class RestaurantModel
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
        public IEnumerable<DishViewModel> Dishes { get; set; }
    }
}