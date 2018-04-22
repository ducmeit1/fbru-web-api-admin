using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using FBru.DTO;
using FBru.Repository.Entities;
using FBru.Repository.Interfaces;
using FBru.WebAdmin.Helpers;
using FBru.WebAdmin.Models;

namespace FBru.WebAdmin.Controllers
{
    [AuthorizeFilter]
    public class DishesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DishesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Dishes
        public async Task<ActionResult> Index()
        {
            var dishes = await _unitOfWork.Dishes.GetDishesWithRestaurantAndCategory();

            return View(dishes);
        }

        public async Task<ActionResult> Create()
        {
            var categories = await _unitOfWork.Categories.GetCategoriesWithDishes();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            var restaurants = await _unitOfWork.Restaurants.GetRestaurantsWithoutDishes();
            ViewBag.RestaurantId = new SelectList(restaurants, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(
            [Bind(Include = "Name, Description, Price, CategoryId, RestaurantId, KeywordIds")] DishModel model)
        {
            var returnUrl = "/Dishes/Create";

            if (!ModelState.IsValid)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                    message: "Please enter all fields are required!"));

            var categoryId = await _unitOfWork.Categories.SingleOrDefault(c => c.Id == model.CategoryId);

            var restaurantId = await _unitOfWork.Restaurants.SingleOrDefault(r => r.Id == model.RestaurantId);

            if (categoryId == null || restaurantId == null)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.BadRequest,
                    returnUrl));

            var dish = Mapper.Map<DishModel, Dish>(model);

            if (model.KeywordIds.Any())
                foreach (var kw in model.KeywordIds)
                {
                    var findKeyword = await _unitOfWork.Keywords.SingleOrDefault(k => kw == k.Id);
                    dish.Keywords.Add(findKeyword);
                }

            _unitOfWork.Dishes.Add(dish);
            await _unitOfWork.Completed();

            return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Success,
                message: "This dish has been added successfully! You will be moved to next page to add some images.",
                returnUrl: $"/Images/Create/{dish.Id}"));
        }

        public async Task<JsonResult> GetKeywords(string query = null)
        {
            var keywords = await _unitOfWork.Keywords.GetKeywordsWithoutDishes();

            if (!string.IsNullOrWhiteSpace(query))
                keywords = keywords
                    .Where(k => k.Name.Contains(query))
                    .OrderBy(k => k.Id);

            return Json(keywords, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Detail(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var dish = await _unitOfWork.Dishes.GetDishWithFullDetails(id.Value);

            if (dish == null)
                return HttpNotFound();

            return View(dish);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Delete(int? id)
        {
            var returnUrl = "/Dishes";
            if (id == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl),
                    JsonRequestBehavior.AllowGet);
            var dish = await _unitOfWork.Dishes.GetDishWithImagesAndKeywords(id.Value);

            if (dish == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl)
                    , JsonRequestBehavior.AllowGet);

            foreach (var item in dish.Images)
            {
                var filePath = Path.Combine(Server.MapPath("~/"), item.Url);
                if (System.IO.File.Exists(filePath))
                    Task.Run(() => { System.IO.File.Delete(filePath); }).Wait();
            }

            _unitOfWork.Dishes.Remove(dish);
            await _unitOfWork.Completed();

            return Json(MessageAlertCenter.GetMessageAlert(
                MessageAlertType.Success,
                message: "That dish has been removed successfully!",
                returnUrl: returnUrl));
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var dish = await _unitOfWork.Dishes.GetDishWithFullDetails(id.Value);

            if (dish == null)
                return HttpNotFound();

            var dishDto = Mapper.Map<DishDto, DishModel>(dish);

            var categories = await _unitOfWork.Categories.GetCategoriesWithDishes();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", dish.Category.Id);

            var restaurants = await _unitOfWork.Restaurants.GetRestaurantsWithoutDishes();
            ViewBag.RestaurantId = new SelectList(restaurants, "Id", "Name", dish.Restaurant.Id);

            var keywordsId = "";

            foreach (var keyword in dishDto.Keywords)
                keywordsId += $"{keyword.Id} ";

            ViewBag.KeywordIds = keywordsId.Trim();

            return View(dishDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(
            [Bind(Include = "Id, Name, Description, Price, CategoryId, RestaurantId, KeywordIds")] DishModel model)
        {
            var returnUrl = "/Dishes";
            if (!ModelState.IsValid)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                    message: "Please enter all fields are required!"));

            var categoryId = await _unitOfWork.Categories.SingleOrDefault(c => c.Id == model.CategoryId);

            var restaurantId = await _unitOfWork.Restaurants.SingleOrDefault(r => r.Id == model.RestaurantId);

            var dish = await _unitOfWork.Dishes.Get(model.Id);

            if (categoryId == null || restaurantId == null || dish == null)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.BadRequest,
                    returnUrl));

            dish.Keywords = new List<Keyword>();

            if (model.KeywordIds.Any())
                foreach (var kw in model.KeywordIds)
                {
                    var findKeyword = await _unitOfWork.Keywords.SingleOrDefault(k => kw == k.Id);
                    dish.Keywords.Add(findKeyword);
                }
            dish.CategoryId = model.CategoryId;
            dish.RestaurantId = model.RestaurantId;
            dish.Name = model.Name;
            dish.Price = model.Price;
            dish.Description = model.Description;

            _unitOfWork.Dishes.Update(dish);
            await _unitOfWork.Completed();

            return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Success,
                message: "This dish has been updated successfully!",
                returnUrl: $"/Dishes/Detail/{dish.Id}"));
        }
    }
}