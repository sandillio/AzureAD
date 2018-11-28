using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AzureNetCoreApp.Models;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AzureNetCoreApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            //var redirectUrl = Url.Action(nameof(HomeController.Contact), "Home");
            //return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, WsFederationDefaults.AuthenticationScheme);
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            if(User.Identity.IsAuthenticated)
            {

            }
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Status()
        {
            if (User.Identity.IsAuthenticated)
                RedirectToAction("Contact");
            return View();
        }

        public IActionResult Signin()
        {
            var redirectUrl = Url.Action(nameof(HomeController.Status), "Home");
            return Challenge(new AuthenticationProperties { RedirectUri = "/AzureADDemo" }, WsFederationDefaults.AuthenticationScheme);
        }

        [HttpPost]
        public IActionResult PostSignIn()
        {
            var redirectUrl = Url.Action(nameof(HomeController.Contact), "Home");
            return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, WsFederationDefaults.AuthenticationScheme);

            
        }
        [HttpGet]
        public async Task Logoutasync()
        {
            var redirectUrl = Url.Action(nameof(HomeController.Contact), "Home");

            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignOutAsync(WsFederationDefaults.AuthenticationScheme);
                //
            }
            RedirectToAction(redirectUrl);
        }

        [HttpPost]
        //[Route("logout")]
        [ValidateAntiForgeryToken]
        public async Task Logout()
        {
            var redirectUrl = Url.Action(nameof(HomeController.Contact), "Home");

            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignOutAsync(WsFederationDefaults.AuthenticationScheme);
                //RedirectToAction(redirectUrl);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
