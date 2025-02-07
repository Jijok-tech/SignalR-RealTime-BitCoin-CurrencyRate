using Microsoft.AspNetCore.Mvc;
using RealTimeDashboard.Models;
using System.Diagnostics;

namespace RealTimeDashboard.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
