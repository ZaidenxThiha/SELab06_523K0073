using System.Web.Mvc;
using System.Web.Routing;

namespace ASPNETWebAppMVCStudentApp.Filters
{
    public class RequireLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var skipAuthorization = filterContext.ActionDescriptor
                                      .IsDefined(typeof(AllowAnonymousAttribute), true) ||
                                      filterContext.ActionDescriptor.ControllerDescriptor
                                          .IsDefined(typeof(AllowAnonymousAttribute), true);

            if (skipAuthorization)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            var session = filterContext.HttpContext.Session;
            if (session["UserID"] == null)
            {
                var returnUrl = filterContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = "Account",
                            action = "Login",
                            returnUrl
                        }));
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }
    }
}
