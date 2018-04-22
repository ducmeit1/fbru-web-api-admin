using System.Web.Mvc;
using FBru.Repository.Interfaces;
using FBru.WebAdmin.Helpers;

namespace FBru.WebAdmin.Controllers
{
    [AuthorizeFilter]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWOrk)
        {
            _unitOfWork = unitOfWOrk;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}