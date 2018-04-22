using FBru.DTO;
using FBru.Repository.Entities;
using FBru.Repository.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace FBru.Repository.Repositories
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        public BlogRepository(FBruContext context) : base(context)
        {
        }

        private FBruContext FBruContext => Context as FBruContext;
        public async Task<IQueryable<BlogSimpleDetailDto>> GetBlogsWithSimpleDetails()
        {
            var blogs = await Task.FromResult(FBruContext.Blogs.Select(b => new BlogSimpleDetailDto()
            {
                Id = b.Id,
                Title = b.Title,
                PublishedDate = b.PublishedDate,
                Author = b.Author
            }).OrderBy(b => b.Id));

            return blogs;
        }
    }
}