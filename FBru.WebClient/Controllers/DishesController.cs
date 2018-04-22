using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using FBru.DTO;
using FBru.WebClient.Helpers;
using Newtonsoft.Json;

namespace FBru.WebClient.Controllers
{
    [RoutePrefix("Dishes")]
    public class DishesController : Controller
    {
        private const int MaxPageSize = 8;
        // GET: Dishes

        [Route("Page/{page}")]
        public async Task<JsonResult> GetDishes(int page)
        {
            var client = FBruHttpClient.GetClient();

            var response = await client.GetAsync($"api/dishes/v1/{page}/{MaxPageSize}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var pagingInfo = HeaderParse.FindAndParsePagingInfo(response.Headers);
                var dishes = JsonConvert.DeserializeObject<IEnumerable<DishWithPrice>>(content);
                return Json(new MessageAlertCenter
                {
                    Server = Constants.FBruServer,
                    Data = dishes,
                    CurrentPage = pagingInfo.CurrentPage,
                    TotalPage = pagingInfo.TotalPages
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.DenyGet);
        }

        [Route("Category/{id}/Page/{page}")]
        public async Task<JsonResult> GetDishesByCategory(int id, int page)
        {
            var client = FBruHttpClient.GetClient();

            var response = await client.GetAsync($"api/dishes/category/{id}/page/{page}/{MaxPageSize}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var pagingInfo = HeaderParse.FindAndParsePagingInfo(response.Headers);
                var dishes = JsonConvert.DeserializeObject<IEnumerable<DishWithPrice>>(content);
                return Json(new MessageAlertCenter
                {
                    Server = Constants.FBruServer,
                    Data = dishes,
                    CurrentPage = pagingInfo.CurrentPage,
                    TotalPage = pagingInfo.TotalPages
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.DenyGet);
        }
    }
}