using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiViews.Models;
using mejor_precio_2.API;
using mejor_precio_2.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ApiViews.Controllers
{
    [Authorize (Roles = "Admin, Moderador")]
    public class UserController : MejorPrecioControllerBase
    {
        public IActionResult Index()
        {
            var userListViewModel = new UserListViewModel();
            userListViewModel.UserList = new UserHandler().GetAll();

            return View(userListViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromForm]UserViewModel user)
        {
            new UserHandler().Create(user);

            return RedirectToAction("Index");
        }

        public IActionResult Update(Guid id)
        {
            var user = new UserHandler().GetOneById(id);

            var userViewModel = new UserViewModel(user);

            return View(userViewModel);
        }

        [HttpPost]
        public IActionResult Update([FromForm]UserViewModel user)
        {
            //user.Id = new Guid(this.User.FindFirstValue(ClaimTypes.Sid));
            new UserHandler().Update(user);
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult UpdateForUser([FromForm]UserViewModel user)
        {

            if(new UserHandler().InfoUpdate(user)){
                return RedirectToAction("Index", "Profile");
            }
            else {
                return RedirectToAction("Logout", "Login");
            }
        }

        public IActionResult Delete(Guid id)
        {
            new UserHandler().Delete(id);

            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
