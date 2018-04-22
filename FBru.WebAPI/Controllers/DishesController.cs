using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using FBru.Repository.Interfaces;
using Newtonsoft.Json;

namespace FBru.WebAPI.Controllers
{
    [RoutePrefix("api")]
    public class DishesController : ApiController
    {
        private const int MaxPageSize = 10;
        private readonly IUnitOfWork _unitOfWork;

        public DishesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("dishes/v1/{page}/{pageSize}")]
        public async Task<IHttpActionResult> Get(int page = 1, int pageSize = MaxPageSize)
        {
            var dishes = await _unitOfWork.Dishes.GetDishesWithPrice();

            var dishesArray = dishes.ToArray();

            var totalCount = dishesArray.Count();
            var totalPages = (int) Math.Ceiling((double) totalCount / pageSize);

            var paginationHeader = new
            {
                currentPage = page,
                pageSize,
                totalCount,
                totalPages
            };

            HttpContext.Current.Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(paginationHeader));
            var skip = pageSize * (page - 1);
            var takeLimited = totalCount - skip;
            var take = pageSize;
            if (pageSize > takeLimited)
                take = takeLimited;

            return Ok(dishesArray.Skip(skip).Take(take).ToList());
        }

        [HttpGet]
        [Route("dishes/category/{id}/page/{page}/{pageSize}")]
        public async Task<IHttpActionResult> Get(int id, int page = 1, int pageSize = MaxPageSize)
        {
            var dishes = await _unitOfWork.Dishes.GetDishesWithPriceByCategory(id);

            var dishesArray = dishes.ToArray();

            var totalCount = dishesArray.Count();
            var totalPages = (int) Math.Ceiling((double) totalCount / pageSize);

            var paginationHeader = new
            {
                currentPage = page,
                pageSize,
                totalCount,
                totalPages
            };

            HttpContext.Current.Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(paginationHeader));
            var skip = pageSize * (page - 1);
            var takeLimited = totalCount - skip;
            var take = pageSize;
            if (pageSize > takeLimited)
                take = takeLimited;

            return Ok(dishesArray.Skip(skip).Take(take).ToList());
        }
    }
}