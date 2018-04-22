using System.Web;
using System.Web.Mvc;

namespace FBru.WebAdmin.Helpers
{
    public class AllowAnonymousFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["Admin"] != null)
                filterContext.Result = new RedirectResult("~/Home/Index");
        }
    }
}