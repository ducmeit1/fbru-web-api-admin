using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FBru.Repository.Entities;
using FBru.Repository.Interfaces;
using FBru.WebAdmin.Helpers;

namespace FBru.WebAdmin.Controllers
{
    [AuthorizeFilter]
    public class ImagesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImagesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Images
        public async Task<ActionResult> Index()
        {
            var images = await _unitOfWork.Images.GetImagesWithDish();

            return View(images);
        }

        public async Task<ActionResult> Create(int? id)
        {
            var dishes = await _unitOfWork.Dishes.GetDishesWithSimpleDetail();

            if (id != null)
            {
                if (dishes.Single(d => d.Id == id.Value) == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                ViewBag.DishId = new SelectList(dishes, "Id", "Name", id);
            }
            else
            {
                ViewBag.DishId = new SelectList(dishes, "Id", "Name");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(
            [Bind(Include = "DishId, Name, IsDisplay")] Image model)
        {
            var returnUrl = "/Images";

            var filesKey = Request.Files.AllKeys;

            if (!ModelState.IsValid || !filesKey.Any())
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                    message: @"Please enter all fields are required!"), JsonRequestBehavior.AllowGet);

            var dish = await _unitOfWork.Dishes.Get(model.DishId);

            if (dish == null)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.BadRequest, "/Images/Create"),
                    JsonRequestBehavior.AllowGet);

            var httpPostedFileBases = new HttpPostedFileBase[filesKey.Length];

            for (var i = 0; i < filesKey.Length; i++)
            {
                var imageUpload = Request.Files[filesKey[i]];
                var validateFile = Utilities.HttpPostedFileBaseFilter(imageUpload);
                if (validateFile >= 0)
                {
                    var error = Utilities.HttpPostedFileBaseErrorMessage(validateFile);
                    return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                        message: error), JsonRequestBehavior.AllowGet);
                }
                httpPostedFileBases[i] = Request.Files[filesKey[i]];
            }

            var originalDirectory = new DirectoryInfo(Server.MapPath("~/Content/Dishes"));
            var directoryPath = Path.Combine(originalDirectory.ToString(), dish.Name);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            foreach (var imageUpload in httpPostedFileBases)
                try
                {
                    var fileName = Guid.NewGuid() + ".jpg";
                    imageUpload.SaveAs(Path.Combine(directoryPath, fileName));
                    var image = new Image
                    {
                        DishId = model.DishId,
                        Name = model.Name,
                        IsDisplay = model.IsDisplay,
                        Url = $"Content/Dishes/{dish.Name}/{fileName}"
                    };
                    _unitOfWork.Images.Add(image);
                }
                catch (Exception)
                {
                    foreach (var image in httpPostedFileBases)
                    {
                        var filePath = Path.Combine(directoryPath, image.FileName);
                        if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
                    }
                    return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.ServerError,
                        "/Images/Create"), JsonRequestBehavior.AllowGet);
                }

            await _unitOfWork.Completed();

            return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Success,
                    message: @"These images added to this dish successfully!", returnUrl: returnUrl),
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Delete(int? id)
        {
            var returnUrl = "/Images";

            if (id == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl),
                    JsonRequestBehavior.AllowGet);

            var image = await _unitOfWork.Images.Get(id.Value);

            if (image == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        message: "That image is not found!",
                        returnUrl: returnUrl)
                    , JsonRequestBehavior.AllowGet);

            var filePath = Path.Combine(Server.MapPath("~/"), image.Url);
            _unitOfWork.Images.Remove(image);

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            await _unitOfWork.Completed();

            return Json(MessageAlertCenter.GetMessageAlert(
                MessageAlertType.Success,
                message: "That image has been removed!",
                returnUrl: returnUrl));
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var image = await _unitOfWork.Images.Get(id.Value);

            if (image == null) return HttpNotFound();

            var dishes = await _unitOfWork.Dishes.GetDishesWithSimpleDetail();

            ViewBag.DishId = new SelectList(dishes, "Id", "Name", image.DishId);

            return View(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit([Bind(Include = "Id, Name, IsDisplay, DishId")] Image model)
        {
            var returnUrl = "/Images";

            if (!ModelState.IsValid)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                    message: @"Please enter all fields are required!"), JsonRequestBehavior.AllowGet);

            var image = await _unitOfWork.Images.Get(model.Id);

            var dish = await _unitOfWork.Dishes.Get(model.DishId);

            if (dish == null || image == null)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.BadRequest,
                    returnUrl), JsonRequestBehavior.AllowGet);

            var imageUpload = Request.Files["FileEdit"];

            if (imageUpload != null)
            {
                var validateFile = Utilities.HttpPostedFileBaseFilter(imageUpload);
                if (validateFile >= 0)
                {
                    var error = Utilities.HttpPostedFileBaseErrorMessage(validateFile);
                    return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                        message: error), JsonRequestBehavior.AllowGet);
                }

                var originalDirectory = new DirectoryInfo(Server.MapPath("~/Content/Dishes"));
                var directoryPath = Path.Combine(originalDirectory.ToString(), dish.Name);
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                try
                {
                    var fileName = Guid.NewGuid() + ".jpg";
                    imageUpload.SaveAs(Path.Combine(directoryPath, fileName));
                    var oldFilePath = Path.Combine(Server.MapPath("~/"), image.Url);
                    if (System.IO.File.Exists(oldFilePath))
                        Task.Run(() => { System.IO.File.Delete(oldFilePath); }).Wait();
                    image.Url = $"Content/Dishes/{dish.Name}/{fileName}";
                }
                catch (Exception)
                {
                    return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.ServerError,
                        $"/Restaurants/Edit/{image.Id}"), JsonRequestBehavior.AllowGet);
                }
            }

            image.Name = model.Name;
            image.DishId = model.DishId;
            image.IsDisplay = model.IsDisplay;
            _unitOfWork.Images.Update(image);
            await _unitOfWork.Completed();

            return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Success,
                    message: @"This image has been updated successfully", returnUrl: returnUrl),
                JsonRequestBehavior.AllowGet);
        }
    }
}