using System.Threading.Tasks;
using System.Web.Mvc;
using FBru.Repository.Interfaces;
using FBru.WebAdmin.Helpers;

namespace FBru.WebAdmin.Controllers
{
    [AuthorizeFilter]
    public class UserGroupsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserGroupsController(IUnitOfWork unitOfWOrk)
        {
            _unitOfWork = unitOfWOrk;
        }

        public async Task<ActionResult> Index()
        {
            var userGroups = await _unitOfWork.UserGroups.GetUserGroupsWithUsersCounted();
            return View(userGroups);
        }
    }
}