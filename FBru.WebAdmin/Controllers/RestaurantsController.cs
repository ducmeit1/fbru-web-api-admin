using FBru.Repository.Entities;
using FBru.Repository.Interfaces;
using FBru.WebAdmin.Helpers;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FBru.WebAdmin.Controllers
{
    [AuthorizeFilter]
    public class RestaurantsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RestaurantsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult> Index()
        {
            var restaurants = await _unitOfWork.Restaurants.GetRestaurantsWithNumberOfDishes();

            return View(restaurants);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(
            [Bind(Include = "Name, Address, Description, PhoneNumber, OpenTime, CloseTime, IsHalal")] Restaurant model)
        {
            if (!ModelState.IsValid)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                    message: "Please enter all fields are required!"));
            var imageUpload = Request.Files["FileUpload"];
            if (imageUpload != null)
            {
                var validateFile = Utilities.HttpPostedFileBaseFilter(imageUpload);
                if (validateFile >= 0)
                    return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                        message: Utilities.HttpPostedFileBaseErrorMessage(validateFile)));
                var originalDirectory = new DirectoryInfo(Server.MapPath("~/Content/Restaurants"));
                if (!Directory.Exists(originalDirectory.ToString()))
                    Directory.CreateDirectory(originalDirectory.ToString());
                try
                {
                    var fileName = Guid.NewGuid() + ".jpg";
                    imageUpload.SaveAs(Server.MapPath("~/Content/Restaurants/" + fileName));
                    model.ImageUrl = "Content/Restaurants/" + fileName;
                }
                catch (Exception)
                {
                    return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.ServerError,
                        "/Restaurants/Create"));
                }
            }
            model.CloseTime = new TimeSpan(model.CloseTime.Hours, model.CloseTime.Minutes, 0);
            model.OpenTime = new TimeSpan(model.OpenTime.Hours, model.OpenTime.Minutes, 0);
            _unitOfWork.Restaurants.Add(model);
            await _unitOfWork.Completed();
            return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Success,
                message: "This restaurant has been added successfully!",
                returnUrl: $"/Restaurants/Detail/{model.Id}"));
        }

        public async Task<ActionResult> Detail(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var restaurant = await _unitOfWork.Restaurants.GetRestaurantWithDishesHaveCategoryDto(id.Value);
            if (restaurant == null)
                return HttpNotFound();

            return View(restaurant);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var restaurant = await _unitOfWork.Restaurants.Get(id.Value);
            if (restaurant == null)
                return HttpNotFound();
            return View(restaurant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(
            [Bind(Include = "Id, Name, Address, Description, PhoneNumber, OpenTime, CloseTime, IsHalal")]
            Restaurant model)
        {
            var returnUrl = "/Restaurants";

            if (!ModelState.IsValid)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                    message: "Please enter all fields are required!"));
            var restaurant = await _unitOfWork.Restaurants.Get(model.Id);

            if (restaurant == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl)
                    , JsonRequestBehavior.AllowGet);
            var imageUpload = Request.Files["FileUpload"];
            if (imageUpload != null)
            {
                var validateFile = Utilities.HttpPostedFileBaseFilter(imageUpload);
                if (validateFile >= 0)
                    return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                        message: Utilities.HttpPostedFileBaseErrorMessage(validateFile)));
                try
                {
                    var fileName = Guid.NewGuid() + ".jpg";
                    imageUpload.SaveAs(Server.MapPath("~/Content/Restaurants/" + fileName));
                    if (restaurant.ImageUrl != null)
                    {
                        var oldFilePath = Server.MapPath($@"~/{restaurant.ImageUrl}");
                        if (System.IO.File.Exists(oldFilePath))
                            Task.Run(() => { System.IO.File.Delete(oldFilePath); }).Wait();
                    }
                    restaurant.ImageUrl = "Content/Restaurants/" + fileName;
                }
                catch (Exception)
                {
                    return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.ServerError,
                        $"/Restaurants/Edit/{restaurant.Id}"));
                }
            }
            restaurant.Name = model.Name;
            restaurant.Description = model.Description;
            restaurant.CloseTime = new TimeSpan(model.CloseTime.Hours, model.CloseTime.Minutes, 0);
            restaurant.OpenTime = new TimeSpan(model.OpenTime.Hours, model.OpenTime.Minutes, 0);
            restaurant.Address = model.Address;
            restaurant.IsHalal = model.IsHalal;
            restaurant.PhoneNumber = model.PhoneNumber;
            _unitOfWork.Restaurants.Update(restaurant);
            await _unitOfWork.Completed();
            return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Success,
                message: "This restaurant has been updated successfully!",
                returnUrl: $"/Restaurants/Detail/{restaurant.Id}"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Delete(int? id)
        {
            var returnUrl = "/Restaurants";
            if (id == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl),
                    JsonRequestBehavior.AllowGet);
            var restaurant = await _unitOfWork.Restaurants.SingleOrDefault(r => r.Id == id,
                "Dishes");
            if (restaurant == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl)
                    , JsonRequestBehavior.AllowGet);
            foreach (var dish in restaurant.Dishes)
                foreach (var image in dish.Images)
                {
                    var filePath = Path.Combine(Server.MapPath("~/"), image.Url);
                    if (System.IO.File.Exists(filePath))
                        Task.Run(() => { System.IO.File.Delete(filePath); }).Wait();
                }
            _unitOfWork.Restaurants.Remove(restaurant);
            await _unitOfWork.Completed();
            return Json(MessageAlertCenter.GetMessageAlert(
                    MessageAlertType.Success,
                    message: "That restaurant has been deleted successfully!",
                    returnUrl: returnUrl)
                , JsonRequestBehavior.AllowGet);
        }
    }
}