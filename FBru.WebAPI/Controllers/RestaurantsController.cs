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
    public class RestaurantsController : ApiController
    {
        private const int MaxPageSize = 10;

        private readonly IUnitOfWork _unitOfWork;

        public RestaurantsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Route("restaurants/{id}", Name = "RestaurantForId")]
        public async Task<IHttpActionResult> Get(int? id)
        {
            if (id == null)
                return BadRequest();

            var restaurant = await _unitOfWork.Restaurants.GetRestaurantWithDishesHaveCategoryDto(id.Value);

            if (restaurant == null)
                return NotFound();

            return Ok(restaurant);
        }

        [Route("restaurants/v1/page/{page}/{pageSize}", Name = "RestaurantsList")]
        public async Task<IHttpActionResult> Get(int page = 1, int pageSize = MaxPageSize)
        {
            var restaurants = await _unitOfWork.Restaurants.GetRestaurantsWithNumberOfDishes();

            var restaurantsArray = restaurants.ToArray();

            var totalCount = restaurantsArray.Count();
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

            return Ok(restaurantsArray.Skip(skip).Take(take).ToList());
        }
    }
}