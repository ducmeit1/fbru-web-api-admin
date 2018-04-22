namespace FBru.DTO
{
    public class ImageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDisplay { get; set; }
        public string Url { get; set; }
    }

    public class ImageWithDishDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDisplay { get; set; }
        public string Url { get; set; }
        public DishSimpleDetailDto Dish { get; set; }
    }
}
