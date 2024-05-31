using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311_Part2_ST10144453.Data;
using PROG7311_Part2_ST10144453.Models;
using System.Diagnostics;

namespace PROG7311_Part2_ST10144453.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Part2DbContext _context;

        public HomeController(ILogger<HomeController> logger, Part2DbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email.Equals(email) && u.Password.Equals(password));

            if (user != null)
            {
                if (user.AccountType == "Staff Member")
                {
                    return RedirectToAction("StaffDash", "StaffDashboard", new { email = user.Email });
                }
                else if (user.AccountType == "Farmer")
                {
                    return RedirectToAction("FarmerDash", "FarmerDashboard", new { userId = user.UserId });
                }
            }

            ViewBag.ErrorMessage = "Invalid email or password";
            return View();
        }



        public IActionResult StaffDashboard()
        {
            return View();
        }

        public IActionResult FarmerDashboard()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
