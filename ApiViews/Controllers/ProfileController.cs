using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiViews.Models;
using mejor_precio_2.API;
using mejor_precio_2.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ApiViews.Controllers
{
    [Authorize]
    public class ProfileController : MejorPrecioControllerBase
    {
        public IActionResult Index()
        {
            var user = new UserHandler().GetOneById(new Guid(this.User.FindFirstValue(ClaimTypes.Sid)));
            user.Password = "oleee";
            user.Salt = "oleeeee";
            var userViewModel = new UserViewModel(user);             
            return View(userViewModel);
        }


    }
}