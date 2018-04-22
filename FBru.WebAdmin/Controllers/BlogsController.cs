using FBru.Repository.Entities;
using FBru.Repository.Interfaces;
using FBru.WebAdmin.Helpers;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FBru.WebAdmin.Controllers
{
    [AuthorizeFilter]
    public class BlogsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlogsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult> Index()
        {
            var blogs = await _unitOfWork.Blogs.GetBlogsWithSimpleDetails();
            return View(blogs);
        }

        public ActionResult Create()
        {
            var blog = new Blog
            {
                PublishedDate = DateTime.Today
            };
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create([Bind(Include = "Title, Description, PublishedDate, Author")] Blog model)
        {
            if (!ModelState.IsValid)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                    message: "Please enter all fields are required!"));
            _unitOfWork.Blogs.Add(model);
            await _unitOfWork.Completed();
            return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Success,
                message: "This blog has been added successfully!",
                returnUrl: $"/Blogs/Detail/{model.Id}"));
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var blog = await _unitOfWork.Blogs.Get(id.Value);
            if (blog == null)
                return HttpNotFound();
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(
            [Bind(Include = "Id, Title, Description, PublishedDate, Author")] Blog model)
        {
            var returnUrl = "/Blogs";
            if (!ModelState.IsValid)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                    message: "Please enter all fields are required!"));
            var blog = await _unitOfWork.Blogs.Get(model.Id);
            if (blog == null)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.BadRequest,
                    returnUrl));
            blog.Description = model.Description;
            blog.PublishedDate = model.PublishedDate;
            blog.Title = model.Title;
            blog.Author = model.Author;
            _unitOfWork.Blogs.Update(blog);
            await _unitOfWork.Completed();
            return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Success,
                message: "This blog has been updated successfully!",
                returnUrl: $"/Blogs/Detail/{blog.Id}"));
        }

        public async Task<ActionResult> Detail(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var blog = await _unitOfWork.Blogs.Get(id.Value);
            if (blog == null)
                return HttpNotFound();
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Delete(int? id)
        {
            var returnUrl = "/Blogs";
            if (id == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl),
                    JsonRequestBehavior.AllowGet);
            var blog = await _unitOfWork.Blogs.Get(id.Value);
            if (blog == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl)
                    , JsonRequestBehavior.AllowGet);
            _unitOfWork.Blogs.Remove(blog);
            await _unitOfWork.Completed();
            return Json(MessageAlertCenter.GetMessageAlert(
                MessageAlertType.Success,
                message: "That blog has been removed successfully!",
                returnUrl: returnUrl));
        }
    }
}