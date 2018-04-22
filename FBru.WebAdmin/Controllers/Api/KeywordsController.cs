using FBru.Repository.Interfaces;
using FBru.WebAdmin.Helpers;
using Microsoft.Ajax.Utilities;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace FBru.WebAdmin.Controllers.Api
{
    [RoutePrefix("api")]
    public class KeywordsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public KeywordsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("keywords/{query?}")]
        public async Task<IHttpActionResult> GetKeywords(string query = null)
        {
            var keywords = await _unitOfWork.Keywords.GetKeywordsWithoutDishes();

            if (!string.IsNullOrWhiteSpace(query))
                keywords = keywords
                    .Where(k => k.Name.Contains(query))
                    .OrderBy(k => k.Id);

            return Ok(keywords);
        }

        [HttpGet]
        [Route("keyword/search/{query}")]
        public async Task<IHttpActionResult> SearchKeyword(string query)
        {
            if (query == null)
            {
                return NotFound();
            }
            var keyword = await _unitOfWork.Keywords.GetKeywordWithDishes(query);
            keyword.Dishes.ForEach(d => d.ImageUrl = Constrains.ServerUrl + d.ImageUrl);
            return Ok(keyword);
        }
    }
}
