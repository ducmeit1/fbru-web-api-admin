using System.Threading.Tasks;
using System.Web.Mvc;
using FBru.Repository.Entities;
using FBru.Repository.Interfaces;
using FBru.WebAdmin.Helpers;
using Utilities = FBru.Repository.Helpers.Utilities;

namespace FBru.WebAdmin.Controllers
{
    [AuthorizeFilter]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWOrk)
        {
            _unitOfWork = unitOfWOrk;
        }

        public async Task<ActionResult> Index()
        {
            var users = await _unitOfWork.Users.GetUsersWithUserGroup();

            return View(users);
        }

        public async Task<ActionResult> Create()
        {
            var groupsId = await _unitOfWork.UserGroups.GetUserGroupsWithoutUsers();
            ViewBag.GroupId = new SelectList(groupsId, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create([Bind(Include = "Email, Password, Name, PhoneNumber, GroupId")] User model)
        {
            var returnUrl = "/Users";
            if (!ModelState.IsValid)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                    message: "Please enter all fields are required!"));
            if (await _unitOfWork.Users.SingleOrDefault(u => u.Email == model.Email) != null)
                return Json(MessageAlertCenter.GetMessageAlert(MessageAlertType.Invalid,
                    message: "This email is already exist! Choose another one"));
            model.Password = Utilities.EncryptStringToMd5(model.Email + model.Password);

            _unitOfWork.Users.Add(model);
            await _unitOfWork.Completed();

            return Json(MessageAlertCenter.GetMessageAlert(
                MessageAlertType.Success,
                message: "This user has been added successfully!",
                returnUrl: returnUrl));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Delete(int? id)
        {
            var returnUrl = "/Users";

            if (id == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl),
                    JsonRequestBehavior.AllowGet);

            var user = await _unitOfWork.Users.Get(id.Value);
            if (user == null)
                return Json(MessageAlertCenter.GetMessageAlert(
                        MessageAlertType.BadRequest,
                        returnUrl),
                    JsonRequestBehavior.AllowGet);

            _unitOfWork.Users.Remove(user);
            await _unitOfWork.Completed();

            return Json(MessageAlertCenter.GetMessageAlert(
                    MessageAlertType.Success,
                    message: "That user has been deleted successfully!",
                    returnUrl: returnUrl)
                , JsonRequestBehavior.AllowGet);
        }
    }
}