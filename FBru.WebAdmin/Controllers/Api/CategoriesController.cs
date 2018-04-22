using FBru.DTO;
using FBru.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace FBru.WebAdmin.Controllers.Api
{
    [RoutePrefix("api")]
    public class CategoriesController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [Route("categories")]
        public async Task<IEnumerable<CategoryWithIconDto>> GetCategories()
        {
            var categories = await _unitOfWork.Categories.GetCategoriesWithIcon();

            return categories;
        }
    }
}