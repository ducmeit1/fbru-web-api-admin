using FBru.Repository.Interfaces;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FBru.WebAdmin.Controllers.Api
{
    [RoutePrefix("api")]
    public class BlogsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlogsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("blogs")]
        public async Task<IHttpActionResult> GetBlogs()
        {
            var blogs = await _unitOfWork.Blogs.GetAll();
            blogs.ForEach(b => b.Description = HttpUtility.HtmlDecode(b.Description));
            return Ok(blogs);
        }

        [HttpGet]
        [Route("blogs/{id}")]
        public async Task<IHttpActionResult> GetBlog(int id)
        {
            var blog = await _unitOfWork.Blogs.Get(id);

            blog.Description = HttpUtility.HtmlDecode(blog.Description);

            return Ok(blog);
        }
    }
}
