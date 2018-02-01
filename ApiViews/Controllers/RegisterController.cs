using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiViews.Models;
using System.Security.Claims;
using mejor_precio_2.API;

namespace ApiViews.Controllers
{
    public class RegisterController : MejorPrecioControllerBase
    {
        [HttpGet]
        public IActionResult Index(Guid token)
        {
            if (new RegisterHandler().RegisterUser(token))
            {
                this.ViewBag.Status = true;
                this.ViewData["TokenMessg"] = "Usuario registrado exitosamente!";
                return View();
            }
            else
            {
                this.ViewBag.Status = false;
                this.ViewData["TokenMessg"] = "Token invalido.";
                return View();
            }
        }
        public IActionResult PassChanger(Guid passToken, string email)
        {
            if (new RegisterHandler().PassTokenCheck(passToken))
            {   
                this.ViewBag.Status = true;
                this.ViewData["Email"] = email;
                return View();
            }
            else
            {
                this.ViewBag.Status = false;
                return View();
            }
        }
        public IActionResult Send (){
            var id = new Guid(this.User.FindFirstValue(ClaimTypes.Sid));
            new MailHandler().SendRecoveryMail(id);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ChangePassword ([FromForm] string password, string email){
            var userHandler = new UserHandler();
            var id = userHandler.GetOneByEmail(email).Id;

            
            if (userHandler.UpdatePassword(id, password)){
                return RedirectToAction("Index", "Home");
            }else{
                this.ViewBag.Status = false;
                return RedirectToAction("Index");
            }

        }


    }
}
