using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using FBru.DTO;
using FBru.WebClient.Helpers;
using Newtonsoft.Json;

namespace FBru.WebClient.Controllers
{
    [RoutePrefix("Restaurants")]
    public class RestaurantsController : Controller
    {
        private const int MaxPageSize = 3;

        // GET: Restaurants
        public ActionResult Index()
        {
            return View();
        }

        [Route("Page/{page}")]
        public async Task<JsonResult> GetRestaurants(int page)
        {
            var client = FBruHttpClient.GetClient();

            var response = await client.GetAsync($"api/restaurants/v1/page/{page}/{MaxPageSize}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var pagingInfo = HeaderParse.FindAndParsePagingInfo(response.Headers);
                var restaurants = JsonConvert.DeserializeObject<IEnumerable<RestaurantWithNumberOfDishes>>(content);
                return Json(new MessageAlertCenter
                {
                    Server = Constants.FBruServer,
                    Data = restaurants,
                    CurrentPage = pagingInfo.CurrentPage,
                    TotalPage = pagingInfo.TotalPages
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.DenyGet);
        }

        public async Task<ActionResult> Detail(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var client = FBruHttpClient.GetClient();

            var response = await client.GetAsync($"api/restaurants/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var restaurant = JsonConvert.DeserializeObject<RestaurantWithDishesHaveCategoryDto>(content);
                return View(restaurant);
            }
            return HttpNotFound();
        }
    }
}