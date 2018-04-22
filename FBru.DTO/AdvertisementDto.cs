namespace FBru.DTO
{
    public class AdvertisementSimpleDetailDto
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ImageUrl { get; set; }
        public string ActionUrl { get; set; }
    }

    public class AdvertisementMoreDetailsDto
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public bool IsDisplay { get; set; }
    }
}
