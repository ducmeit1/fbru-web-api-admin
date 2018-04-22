namespace FBru.DTO
{
    public class UserGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UserGroupWithUserCountedDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfUsers { get; set; }
    }
}
