using mejor_precio_2.API;
using Microsoft.AspNetCore.Mvc;
using System;
using ApiViews.Models;
using System.Security.Claims;
using mejor_precio_2.Models;
using Microsoft.AspNetCore.Authorization;

namespace ApiViews.Controllers
{
    [Authorize (Roles = "Admin, Moderador")]
    public class NotificationController : MejorPrecioControllerBase
    {       
        public IActionResult Index()
        {
            var idUser = this.User.FindFirstValue(ClaimTypes.Sid);

            var notificationViewModel = new NotificationListViewModel();
            notificationViewModel.NotificationList = new NotificationHandler().GetAllNotifications();

            return View(notificationViewModel);
        }

        [HttpGet]
        public IActionResult Update(Guid id)
        {
            var notification = new NotificationHandler().GetOneById(id);

            var notificationViewModel = new NotificationViewModel(notification);

            return View(notificationViewModel);
        }

        [HttpPost]
        public IActionResult Update(NotificationViewModel notification)
        {
            new PriceHandler().Update(notification.NewPrice);
            new NotificationHandler().Delete(notification.Id);

            return RedirectToAction("Index");
        }
    }
}

