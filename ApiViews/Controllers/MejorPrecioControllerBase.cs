using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;

namespace ApiViews.Controllers
{
    [UserDataFilter]
    public abstract class MejorPrecioControllerBase : Controller
    {
    }

    public class UserDataFilterAttribute : ActionFilterAttribute
    {
        public UserDataFilterAttribute() : base()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as MejorPrecioControllerBase;
            controller.ViewBag.Email = controller.User.FindFirstValue(ClaimTypes.Email);
            controller.ViewBag.Autenticado = controller.User.Identity.IsAuthenticated;
            controller.ViewBag.Name = controller.User.Identity.Name;
            controller.ViewBag.Id = controller.User.FindFirstValue(ClaimTypes.Sid);
            controller.ViewBag.Role = controller.User.FindFirstValue(ClaimTypes.Role);
        }
    }
}