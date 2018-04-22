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
    public class AdvertisementsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdvertisementsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult> Index()
        {
            var advertisements = await _unitOfWork.Advertisements.GetAdvertisementsWithMoreDetailsOrdered();

            return View(advertisements);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create
            ([Bind(Include = "Order, Title, SubTitle, ActionUrl, IsDisplay, Description")] Advertisement model)
        {
            var imageUpload = Request.Files["FileUpload"];
            if (!ModelState.IsValid || imageUpload == null)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                    message: "Please enter all fields are required!"));
            var validateFile = Utilities.HttpPostedFileBaseFilter(imageUpload);
            if (validateFile >= 0)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                    message: Utilities.HttpPostedFileBaseErrorMessage(validateFile)));
            var originalDirectory = new DirectoryInfo(Server.MapPath("~/Content/Advertisements"));
            if (!Directory.Exists(originalDirectory.ToString()))
                Directory.CreateDirectory(originalDirectory.ToString());
            try
            {
                var fileName = Guid.NewGuid() + ".jpg";
                imageUpload.SaveAs(Server.MapPath("~/Content/Advertisements/" + fileName));
                model.ImageUrl = "Content/Advertisements/" + fileName;
            }
            catch (Exception)
            {
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.ServerError,
                    "/Advertisements/Create"));
            }
            _unitOfWork.Advertisements.Add(model);
            await _unitOfWork.Completed();
            return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Success,
                message: "This advertisement has been added successfully!",
                returnUrl: $"/Advertisements/Detail/{model.Id}"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Delete(int? id)
        {
            var returnUrl = "/Advertisements";
            if (id == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl),
                    JsonRequestBehavior.AllowGet);
            var advertisement = await _unitOfWork.Advertisements.Get(id.Value);
            if (advertisement == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl)
                    , JsonRequestBehavior.AllowGet);
            var filePath = Server.MapPath($@"~/{advertisement.ImageUrl}");
            if (!System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
            _unitOfWork.Advertisements.Remove(advertisement);
            await _unitOfWork.Completed();
            return Json(MessageAlertCenter.GetMessageAlert(
                MessageAlertType.Success,
                message: "That advertisement has been removed successfully!",
                returnUrl: returnUrl));
        }

        [HttpGet]
        public async Task<ActionResult> Detail(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var advertisement = await _unitOfWork.Advertisements.Get(id.Value);
            if (advertisement == null)
                return HttpNotFound();
            return View(advertisement);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var advertisement = await _unitOfWork.Advertisements.Get(id.Value);
            if (advertisement == null)
                return HttpNotFound();
            return View(advertisement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "Id, Order, Title, SubTitle, ActionUrl, IsDisplay, Description")] Advertisement model)
        {
            var returnUrl = "/Advertisements";
            if (!ModelState.IsValid)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                    message: "Please enter all fields are required!"));
            var advertisement = await _unitOfWork.Advertisements.Get(model.Id);
            if (advertisement == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl),
                    JsonRequestBehavior.AllowGet);
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
                    imageUpload.SaveAs(Server.MapPath("~/Content/Advertisements/" + fileName));
                    if (advertisement.ImageUrl != null)
                    {
                        var oldFilePath = Server.MapPath($@"~/{advertisement.ImageUrl}");
                        if (System.IO.File.Exists(oldFilePath))
                            Task.Run(() => { System.IO.File.Delete(oldFilePath); }).Wait();
                    }
                    advertisement.ImageUrl = "Content/Advertisements/" + fileName;
                }
                catch (Exception)
                {
                    return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.ServerError,
                        $"/Advertisements/Edit/{advertisement.Id}"));
                }
            }
            advertisement.ActionUrl = model.ActionUrl;
            advertisement.IsDisplay = model.IsDisplay;
            advertisement.Order = model.Order;
            advertisement.SubTitle = model.SubTitle;
            advertisement.Title = model.Title;
            advertisement.Description = model.Description;
            _unitOfWork.Advertisements.Update(advertisement);
            await _unitOfWork.Completed();
            return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Success,
                message: "This advertisement has been updated successfully!",
                returnUrl: $"/Advertisements/Detail/{advertisement.Id}"));
        }
    }
}