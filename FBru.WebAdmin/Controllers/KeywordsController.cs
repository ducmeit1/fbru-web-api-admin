using FBru.Repository.Entities;
using FBru.Repository.Interfaces;
using FBru.WebAdmin.Helpers;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FBru.WebAdmin.Controllers
{
    [AuthorizeFilter]
    public class KeywordsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public KeywordsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult> Index()
        {
            var keywords = await _unitOfWork.Keywords.GetKeywordsWithoutDishes();

            return View(keywords);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name")] Keyword model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (await _unitOfWork.Keywords.SingleOrDefault(k => k.Name.ToLower() == model.Name.ToLower()) != null)
            {
                ModelState.AddModelError(string.Empty, @"This keyword is already exist!");
                return View(model);
            }


            _unitOfWork.Keywords.Add(model);
            await _unitOfWork.Completed();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var keyword = await _unitOfWork.Keywords.Get(id.Value);

            if (keyword == null)
                return HttpNotFound();

            return View(keyword);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit([Bind(Include = "Id, Name")] Keyword model)
        {
            var returnUrl = "/Keywords";

            if (!ModelState.IsValid)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                    message: "Please enter all fields are required!"));

            var keyword = await _unitOfWork.Keywords.Get(model.Id);

            if (keyword == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl)
                    , JsonRequestBehavior.AllowGet);

            keyword.Name = model.Name;
            _unitOfWork.Keywords.Update(keyword);

            await _unitOfWork.Completed();

            return Json(MessageAlertCenter.GetMessageAlert(
                    MessageAlertType.Success,
                    message: "This keyword has been updated successfully!",
                    returnUrl: returnUrl)
                , JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> Detail(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var keyword = await _unitOfWork.Keywords.GetKeywordWithDishes(id.Value);

            if (keyword == null)
                return HttpNotFound();

            return View(keyword);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteDish(int? keywordId, int? dishId)
        {
            var returnUrl = "/Keywords";

            if (keywordId == null || dishId == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl),
                    JsonRequestBehavior.AllowGet);

            var keyword = await _unitOfWork.Keywords.Get(keywordId.Value);

            var dish = await _unitOfWork.Dishes.GetDishWithKeywords(dishId.Value);

            if (keyword == null && dish == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl),
                    JsonRequestBehavior.AllowGet);

            dish.Keywords.Remove(keyword);

            _unitOfWork.Dishes.Update(dish);
            await _unitOfWork.Completed();

            return Json(MessageAlertCenter.GetMessageAlert(
                    MessageAlertType.Success,
                    message: "That dish has been removed from this keyword successfully!")
                , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Delete(int? id)
        {
            var returnUrl = "/Keywords";

            if (id == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl),
                    JsonRequestBehavior.AllowGet);

            var keyword = await _unitOfWork.Keywords.Get(id.Value);

            if (keyword == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl),
                    JsonRequestBehavior.AllowGet);

            _unitOfWork.Keywords.Remove(keyword);
            await _unitOfWork.Completed();

            return Json(MessageAlertCenter.GetMessageAlert(
                    MessageAlertType.Success,
                    message: "This keyword has been removed successfully!",
                    returnUrl: returnUrl)
                , JsonRequestBehavior.AllowGet);
        }
    }
}