using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MagicMVC.Models;
using MagicMVC.Data;
using Microsoft.AspNetCore.Authorization;

namespace MagicMVC.Controllers
{
    [Authorize(Roles = Constants.AllRoles)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Melbourne Magic";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Melbourne Magic";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
