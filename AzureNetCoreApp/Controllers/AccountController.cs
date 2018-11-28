using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Mvc;

namespace AzureNetCoreApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Signin()
        {
            var redirectUrl = Url.Action(nameof(HomeController.Status), "Home");
            return Challenge(new AuthenticationProperties { RedirectUri = "/AzureADDemo" }, WsFederationDefaults.AuthenticationScheme);
        }

        [HttpPost]
        //[Route("logout")]
        [ValidateAntiForgeryToken]
        public async Task Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                var redirectUrl = Url.Action(nameof(HomeController.Contact), "Home");
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignOutAsync(WsFederationDefaults.AuthenticationScheme);
            }
        }
    }
}