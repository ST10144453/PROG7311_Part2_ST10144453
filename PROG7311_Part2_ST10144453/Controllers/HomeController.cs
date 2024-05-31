//-------00000000000000000000oooooooooooooooooooo..........Start of File..........oooooooooooooooooooo00000000000000000000------//
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311_Part2_ST10144453.Data;
using PROG7311_Part2_ST10144453.Models;
using System.Diagnostics;

namespace PROG7311_Part2_ST10144453.Controllers
{
    //------------------------------....................HomeController Class....................------------------------------//
    public class HomeController : Controller
    {
        //oooooooooo............Declarations............oooooooooo//
        private readonly ILogger<HomeController> _logger;
        private readonly Part2DbContext _context;

        //............................................Constructor()............................................//
        /// <summary>
        /// The constructor for the HomeController class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        public HomeController(ILogger<HomeController> logger, Part2DbContext context)
        {
            _logger = logger;
            _context = context;
        }

        //............................................Index()............................................//
        /// <summary>
        /// The Index action method for the HomeController class.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        //............................................Privacy()............................................//
        /// <summary>
        /// The Privacy action method for the HomeController class. 
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }

        //............................................Login()............................................//
        /// <summary>
        /// This is the Login action method for the HomeController class.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //............................................Login()............................................//
        /// <summary>
        /// This is the Login action method for the HomeController class.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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

        //............................................StaffDashboard()............................................//
        /// <summary>
        /// The StaffDashboard action method for the HomeController class.
        /// </summary>
        /// <returns></returns>
        public IActionResult StaffDashboard()
        {
            return View();
        }

        //............................................FarmerDashboard()............................................//
        /// <summary>
        /// The FarmerDashboard action method for the HomeController class.
        /// </summary>
        /// <returns></returns>
        public IActionResult FarmerDashboard()
        {
            return View();
        }

        //............................................Error()............................................// 
        /// <summary>
        /// The Error action method for the HomeController class.
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
//-------00000000000000000000oooooooooooooooooooo..........End of File..........oooooooooooooooooooo00000000000000000000------//
