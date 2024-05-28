using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311_Part2_ST10144453.Data;
using PROG7311_Part2_ST10144453.Models.Domain;

namespace PROG7311_Part2_ST10144453.Controllers
{
    public class StaffDashboard : Controller
    {
        private readonly Part2DbContext _context;

        public StaffDashboard(Part2DbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> StaffDash(string email)
        {
            // Fetch the user from the database using the email.
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            // Pass the user to the view.
            return View(user);
        }


        [HttpGet]
        public IActionResult AddFarmer()
        {
            return View();
        }

    }
}
