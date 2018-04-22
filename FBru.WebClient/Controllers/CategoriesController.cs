using FBru.DTO;
using FBru.WebClient.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FBru.WebClient.Controllers
{
    public class CategoriesController : Controller
    {
        // GET: Category
        public async Task<JsonResult> GetCategories()
        {
            var client = FBruHttpClient.GetClient();

            var response = await client.GetAsync("api/categories");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<IEnumerable<CategoryWithIconDto>>(content);
                return Json(categories, JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.DenyGet);
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return View(id);
        }
    }
}