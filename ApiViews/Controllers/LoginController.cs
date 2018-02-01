using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using mejor_precio_2.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Threading;
using Microsoft.AspNetCore.Authentication.Cookies;
using mejor_precio_2.API;
using ApiViews.Models;

namespace ApiViews.Controllers
{
    public class LoginController : MejorPrecioControllerBase
    {
        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ValidateLogin(model.Email, model.Password))
            {
                this.ModelState.AddModelError("", "Email o contraseña incorrecto");
            }

            if (ModelState.IsValid)
            {
                var userEmailClaim = new Claim(ClaimTypes.Email, model.Email);
                var user = GetUser(model.Email);
                var roleClaim = new Claim(ClaimTypes.Role, user.Type);
                var userNameClaim = new Claim(ClaimTypes.Name, user.Name);
                var userIdClaim = new Claim(ClaimTypes.Sid, user.Id.ToString());
                var identity = new ClaimsIdentity(new[] { userEmailClaim, roleClaim, userNameClaim, userIdClaim }, "cookie");
                var principal = new ClaimsPrincipal(identity);

                await this.HttpContext.SignInAsync(principal);
                // hacer login

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(model);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await this.HttpContext.SignOutAsync();

            return RedirectToAction("Index");
        }


        private bool ValidateLogin(string email, string password)
        {
            var login = new LoginHandler().CompareLoginCredentials(email, password);
            return login;
        }

        private UserInfo GetUser(string email)
        {
            try
            {
                var user = new UserHandler().GetOneByEmail(email).UserInfoCreate();

                return user;
            }
            catch
            {
                return null;
            }
        }

        public IActionResult PasswordRecovery()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PasswordRecoverySendMail(string email)
        {

            var mailHandler = new MailHandler();
            var id = new UserHandler().GetOneByEmail(email).Id;
            try
            {
                mailHandler.SendRecoveryMail(id);
                return View("PasswordSended");
            }
            catch
            {
                return View("MailNotValid");
            }
        }

        public IActionResult ErrorLogin()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            var userHandler = new UserHandler();
            model.Type = "Cliente";
            try
            {
                if (!VerifyPassword(model.Password, model.ConfirmPassword))
                {
                    return View(model);
                }
                var user = userHandler.Create(model);

            }
            catch
            {
                this.ModelState.AddModelError("emailInvalid", "El mail ha sido registrado anteriormente.");
                return View(model);
            }



            return RedirectToAction("Index");
        }

        private bool VerifyPassword(string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                this.ModelState.AddModelError("passwordInvalid", "Password don't match");
                return false;
            }
            return true;
        }
    }
}
