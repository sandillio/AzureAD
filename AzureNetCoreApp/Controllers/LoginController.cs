using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Mvc;

namespace AzureNetCoreApp.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            
            return View();
        }
    }
}