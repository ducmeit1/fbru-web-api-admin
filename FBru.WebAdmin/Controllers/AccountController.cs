using System.Threading.Tasks;
using System.Web.Mvc;
using FBru.Repository.Interfaces;
using FBru.WebAdmin.Helpers;
using FBru.WebAdmin.Models;
using Utilities = FBru.Repository.Helpers.Utilities;

namespace FBru.WebAdmin.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWOrk)
        {
            _unitOfWork = unitOfWOrk;
        }

        [AllowAnonymousFilter]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymousFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);
            var passwordEncrypted = Utilities.EncryptStringToMd5(model.Email + model.Password);
            var user = await _unitOfWork.Users.SingleOrDefault(
                u => u.Email == model.Email &&
                     u.Password == passwordEncrypted);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, @"Email or Password is invalid, try again!");
                return View("Index", model);
            }
            if (user.GroupId != 1)
            {
                ModelState.AddModelError(string.Empty, @"Invalid account!");
                return View("Index", model);
            }
            Session["Admin"] = user.Email;
            return RedirectToAction("Index", "Home");
        }

        [AuthorizeFilter]
        public ActionResult Logout()
        {
            if (Session["Admin"] != null)
                Session.Abandon();
            return RedirectToAction("Index");
        }
    }
}