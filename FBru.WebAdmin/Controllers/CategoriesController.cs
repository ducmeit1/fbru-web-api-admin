using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using FBru.Repository.Entities;
using FBru.Repository.Interfaces;
using FBru.WebAdmin.Helpers;

namespace FBru.WebAdmin.Controllers
{
    [AuthorizeFilter]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult> Index()
        {
            var categories = await _unitOfWork.Categories.GetCategoriesWithoutDishes();

            return View(categories);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create([Bind(Include = "Name, Icon")] Category model)
        {
            if (!ModelState.IsValid)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                    message: "Please enter all fields are required!"));

            if (await _unitOfWork.Categories.SingleOrDefault(c => c.Name.ToLower() == model.Name.ToLower()) != null)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                    message: "This category name is already exist, try again!"));

            _unitOfWork.Categories.Add(model);

            await _unitOfWork.Completed();

            return Json(MessageAlertCenter.GetMessageAlert(
                MessageAlertType.Success,
                message: "This category name has been added successfully!",
                returnUrl: $"/Categories/Detail/{model.Id}"));
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var category = await _unitOfWork.Categories.Get(id.Value);

            if (category == null)
                return HttpNotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit([Bind(Include = "Id, Name, Icon")] Category model)
        {
            var returnUrl = "/Categories";

            if (!ModelState.IsValid)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                    message: "Please enter all fields are required!"));

            var category = await _unitOfWork.Categories.Get(model.Id);

            if (category == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl)
                    , JsonRequestBehavior.AllowGet);

            category.Name = model.Name;
            category.Icon = model.Icon;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.Completed();

            return Json(MessageAlertCenter.GetMessageAlert(
                    MessageAlertType.Success,
                    message: "This category has been updated successfully!",
                    returnUrl: $"/Categories/Detail/{category.Id}")
                , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Delete(int? id)
        {
            var returnUrl = "/Categories";
            if (id == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl),
                    JsonRequestBehavior.AllowGet);
            var category = await _unitOfWork.Categories.GetCategoryWithDishes(id.Value);

            if (category == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl)
                    , JsonRequestBehavior.AllowGet);

            foreach (var dish in category.Dishes)
            foreach (var image in dish.Images)
            {
                var filePath = Path.Combine(Server.MapPath("~/"), image.Url);
                if (System.IO.File.Exists(filePath))
                    Task.Run(() => { System.IO.File.Delete(filePath); }).Wait();
            }

            _unitOfWork.Categories.Remove(category);
            await _unitOfWork.Completed();

            return Json(MessageAlertCenter.GetMessageAlert(
                    MessageAlertType.Success,
                    message: "That category has been deleted successfully!",
                    returnUrl: returnUrl)
                , JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> Detail(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var category = await _unitOfWork.Categories.GetCategoryWithDishesHaveRestaurants(id.Value);

            if (category == null)
                return HttpNotFound();

            return View(category);
        }
    }
}