using System;

namespace FBru.DTO
{
    public class BlogSimpleDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
    }
}
